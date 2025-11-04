using System.ComponentModel.DataAnnotations;

namespace ParallelLab.Core.Entities;

public class ExerciseSubmission
{
    public int Id { get; set; }
    
    public int ExerciseId { get; set; }
    
    [Required]
    public string UserCode { get; set; } = string.Empty;
    
    public string? CompilationError { get; set; }
    
    public string? RuntimeError { get; set; }
    
    public long ExecutionTimeMs { get; set; }
    
    public bool IsCorrect { get; set; }
    
    public string? Output { get; set; }
    
    public double PerformanceScore { get; set; }
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    
    public string? UserId { get; set; }
    
    // Navigation properties
    public Exercise Exercise { get; set; } = null!;
    public ICollection<TestCaseResult> TestCaseResults { get; set; } = new List<TestCaseResult>();
}

public class PerformanceComparison
{
    public int Id { get; set; }
    
    public int SubmissionId { get; set; }
    
    public long UserExecutionTimeMs { get; set; }
    
    public long IdealExecutionTimeMs { get; set; }
    
    public double PerformanceRatio { get; set; }
    
    public string Analysis { get; set; } = string.Empty;
    
    public DateTime ComparedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ExerciseSubmission Submission { get; set; } = null!;
}


