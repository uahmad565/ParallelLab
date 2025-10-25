using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;

namespace ParallelLab.Infrastructure.Data;

public class ParallelLabDbContext : DbContext
{
    public ParallelLabDbContext(DbContextOptions<ParallelLabDbContext> options) : base(options)
    {
    }

    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseSubmission> ExerciseSubmissions { get; set; }
    public DbSet<PerformanceComparison> PerformanceComparisons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}


