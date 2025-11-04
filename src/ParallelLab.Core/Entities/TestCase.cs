using System.ComponentModel.DataAnnotations;

namespace ParallelLab.Core.Entities;

public class TestCase
{
    public int Id { get; set; }
    
    public int ExerciseId { get; set; }
    
    [Required]
    public string Input { get; set; } = string.Empty;
    
    [Required]
    public string ExpectedOutput { get; set; } = string.Empty;
    
    public int TimeoutMs { get; set; } = 5000;
    
    public int IdealExecutionTimeMs { get; set; }
    
    public bool IsHidden { get; set; } = false;
    
    public int Order { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Exercise Exercise { get; set; } = null!;
}

public class TestCaseResult
{
    public int Id { get; set; }
    
    public int SubmissionId { get; set; }
    
    public int TestCaseId { get; set; }
    
    public bool Passed { get; set; }
    
    public string ActualOutput { get; set; } = string.Empty;
    
    public string ExpectedOutput { get; set; } = string.Empty;
    
    public long ExecutionTimeMs { get; set; }
    
    public bool TimedOut { get; set; }
    
    public int ExitCode { get; set; }
    
    public string? StandardError { get; set; }
    
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ExerciseSubmission Submission { get; set; } = null!;
    public TestCase TestCase { get; set; } = null!;
}

