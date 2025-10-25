using System.ComponentModel.DataAnnotations;

namespace ParallelLab.Core.Entities;

public class Exercise
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string ProblemStatement { get; set; } = string.Empty;
    
    [Required]
    public string StartingCode { get; set; } = string.Empty;
    
    [Required]
    public string IdealSolution { get; set; } = string.Empty;
    
    [Required]
    public string TestData { get; set; } = string.Empty;
    
    public ExerciseCategory Category { get; set; }
    
    public DifficultyLevel Difficulty { get; set; }
    
    public int ExpectedExecutionTimeMs { get; set; }
    
    public int MaxExecutionTimeMs { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<ExerciseSubmission> Submissions { get; set; } = new List<ExerciseSubmission>();
}

public enum ExerciseCategory
{
    Threads,
    Tasks,
    LINQ,
    ParallelFor,
    ConcurrentCollections,
    AsyncAwait,
    PLINQ
}

public enum DifficultyLevel
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}


