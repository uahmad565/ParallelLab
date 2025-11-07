using System.ComponentModel.DataAnnotations;

namespace ParallelLab.Core.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    public UserRole Role { get; set; } = UserRole.User;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<ExerciseSubmission> Submissions { get; set; } = new List<ExerciseSubmission>();
}

public enum UserRole
{
    User = 0,           // Can only access Beginner exercises
    PremiumUser = 1,    // Can access all exercises
    Admin = 2           // Full access + admin features
}

