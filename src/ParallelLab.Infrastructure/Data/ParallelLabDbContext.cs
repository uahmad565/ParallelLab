using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;

namespace ParallelLab.Infrastructure.Data;

public class ParallelLabDbContext : DbContext
{
    public ParallelLabDbContext(DbContextOptions<ParallelLabDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseSubmission> ExerciseSubmissions { get; set; }
    public DbSet<PerformanceComparison> PerformanceComparisons { get; set; }
    public DbSet<TestCase> TestCases { get; set; }
    public DbSet<TestCaseResult> TestCaseResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Exercise configuration
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.ProblemStatement).IsRequired();
            entity.Property(e => e.StartingCode).IsRequired();
            entity.Property(e => e.IdealSolution).IsRequired();
            entity.Property(e => e.TestData).IsRequired();
            entity.Property(e => e.Category).HasConversion<string>();
            entity.Property(e => e.Difficulty).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // ExerciseSubmission configuration
        modelBuilder.Entity<ExerciseSubmission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserCode).IsRequired();
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Exercise)
                  .WithMany(e => e.Submissions)
                  .HasForeignKey(e => e.ExerciseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PerformanceComparison configuration
        modelBuilder.Entity<PerformanceComparison>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ComparedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Submission)
                  .WithMany()
                  .HasForeignKey(e => e.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TestCase configuration
        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Input).IsRequired();
            entity.Property(e => e.ExpectedOutput).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Exercise)
                  .WithMany(e => e.TestCases)
                  .HasForeignKey(e => e.ExerciseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TestCaseResult configuration
        modelBuilder.Entity<TestCaseResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExecutedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.Submission)
                  .WithMany(e => e.TestCaseResults)
                  .HasForeignKey(e => e.SubmissionId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.TestCase)
                  .WithMany()
                  .HasForeignKey(e => e.TestCaseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}


