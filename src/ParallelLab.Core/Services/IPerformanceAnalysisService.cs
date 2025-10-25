using ParallelLab.Core.Entities;

namespace ParallelLab.Core.Services;

public interface IPerformanceAnalysisService
{
    Task<PerformanceAnalysisResult> AnalyzePerformanceAsync(
        ExerciseSubmission submission, 
        long idealExecutionTimeMs);
    
    Task<PerformanceComparison> CompareWithIdealAsync(
        ExerciseSubmission submission, 
        string idealSolution, 
        string testData);
}

public class PerformanceAnalysisResult
{
    public double PerformanceScore { get; set; }
    public string Analysis { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = new();
    public PerformanceLevel Level { get; set; }
}

public enum PerformanceLevel
{
    Poor,
    BelowAverage,
    Average,
    Good,
    Excellent
}


