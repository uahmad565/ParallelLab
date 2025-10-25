using Microsoft.Extensions.Logging;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Services;

namespace ParallelLab.Infrastructure.Services;

public class PerformanceAnalysisService : IPerformanceAnalysisService
{
    private readonly ILogger<PerformanceAnalysisService> _logger;

    public PerformanceAnalysisService(ILogger<PerformanceAnalysisService> logger)
    {
        _logger = logger;
    }

    public async Task<PerformanceAnalysisResult> AnalyzePerformanceAsync(
        ExerciseSubmission submission, 
        long idealExecutionTimeMs)
    {
        var performanceRatio = (double)submission.ExecutionTimeMs / idealExecutionTimeMs;
        var performanceScore = CalculatePerformanceScore(performanceRatio);
        var level = DeterminePerformanceLevel(performanceScore);
        var analysis = GenerateAnalysis(performanceRatio, level);
        
        // Use a default category if Exercise is not loaded
        var category = submission.Exercise?.Category ?? ExerciseCategory.Threads;
        var recommendations = GenerateRecommendations(performanceRatio, level, category);

        return await Task.FromResult(new PerformanceAnalysisResult
        {
            PerformanceScore = performanceScore,
            Analysis = analysis,
            Recommendations = recommendations,
            Level = level
        });
    }

    public async Task<PerformanceComparison> CompareWithIdealAsync(
        ExerciseSubmission submission, 
        string idealSolution, 
        string testData)
    {
        // This would typically execute the ideal solution and compare
        // For now, we'll use the expected execution time from the exercise
        // If Exercise is not loaded, we'll use a default value
        var idealExecutionTime = submission.Exercise?.ExpectedExecutionTimeMs ?? 100;
        var performanceRatio = (double)submission.ExecutionTimeMs / idealExecutionTime;
        
        var analysis = await AnalyzePerformanceAsync(submission, idealExecutionTime);
        
        var comparison = new PerformanceComparison
        {
            SubmissionId = submission.Id,
            UserExecutionTimeMs = submission.ExecutionTimeMs,
            IdealExecutionTimeMs = idealExecutionTime,
            PerformanceRatio = performanceRatio,
            Analysis = analysis.Analysis
        };

        return comparison;
    }

    private double CalculatePerformanceScore(double performanceRatio)
    {
        // Score is better when ratio is closer to 1 (ideal performance)
        // Score ranges from 0 to 100
        if (performanceRatio <= 1.0)
        {
            return 100.0; // Perfect or better than ideal
        }
        else if (performanceRatio <= 2.0)
        {
            return 100.0 - ((performanceRatio - 1.0) * 20); // 80-100 for 1-2x ideal time
        }
        else if (performanceRatio <= 5.0)
        {
            return 80.0 - ((performanceRatio - 2.0) * 15); // 35-80 for 2-5x ideal time
        }
        else
        {
            return Math.Max(0, 35.0 - ((performanceRatio - 5.0) * 5)); // 0-35 for >5x ideal time
        }
    }

    private PerformanceLevel DeterminePerformanceLevel(double performanceScore)
    {
        return performanceScore switch
        {
            >= 90 => PerformanceLevel.Excellent,
            >= 75 => PerformanceLevel.Good,
            >= 60 => PerformanceLevel.Average,
            >= 40 => PerformanceLevel.BelowAverage,
            _ => PerformanceLevel.Poor
        };
    }

    private string GenerateAnalysis(double performanceRatio, PerformanceLevel level)
    {
        var timeComparison = performanceRatio switch
        {
            <= 1.0 => "Your solution is as fast or faster than the ideal solution!",
            <= 1.5 => "Your solution is very close to optimal performance.",
            <= 2.0 => "Your solution has good performance with room for improvement.",
            <= 3.0 => "Your solution has moderate performance. Consider optimization.",
            <= 5.0 => "Your solution has below-average performance. Significant optimization needed.",
            _ => "Your solution has poor performance. Major refactoring recommended."
        };

        return $"{timeComparison} Performance ratio: {performanceRatio:F2}x ideal time. Level: {level}";
    }

    private List<string> GenerateRecommendations(double performanceRatio, PerformanceLevel level, ExerciseCategory category)
    {
        var recommendations = new List<string>();

        if (level == PerformanceLevel.Poor || level == PerformanceLevel.BelowAverage)
        {
            recommendations.Add("Consider using parallel processing techniques");
            recommendations.Add("Review your algorithm's time complexity");
            recommendations.Add("Look for opportunities to reduce unnecessary computations");
        }

        switch (category)
        {
            case ExerciseCategory.Threads:
                recommendations.Add("Ensure proper thread synchronization");
                recommendations.Add("Consider using thread pools for better resource management");
                break;
            case ExerciseCategory.Tasks:
                recommendations.Add("Use async/await patterns effectively");
                recommendations.Add("Consider Task.Run for CPU-bound operations");
                break;
            case ExerciseCategory.LINQ:
                recommendations.Add("Use PLINQ for parallel LINQ operations");
                recommendations.Add("Consider deferred execution benefits");
                break;
            case ExerciseCategory.ParallelFor:
                recommendations.Add("Ensure work is evenly distributed across threads");
                recommendations.Add("Consider using Partitioner for custom partitioning");
                break;
            case ExerciseCategory.ConcurrentCollections:
                recommendations.Add("Choose the right concurrent collection for your use case");
                recommendations.Add("Minimize lock contention");
                break;
        }

        return recommendations;
    }
}


