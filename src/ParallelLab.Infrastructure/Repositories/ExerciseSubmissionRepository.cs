using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Interfaces;
using ParallelLab.Infrastructure.Data;

namespace ParallelLab.Infrastructure.Repositories;

public class ExerciseSubmissionRepository : IExerciseSubmissionRepository
{
    private readonly ParallelLabDbContext _context;

    public ExerciseSubmissionRepository(ParallelLabDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseSubmission> CreateAsync(ExerciseSubmission submission)
    {
        _context.ExerciseSubmissions.Add(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task<ExerciseSubmission?> GetByIdAsync(int id)
    {
        return await _context.ExerciseSubmissions
            .Include(s => s.Exercise)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<ExerciseSubmission>> GetByExerciseIdAsync(int exerciseId)
    {
        return await _context.ExerciseSubmissions
            .Where(s => s.ExerciseId == exerciseId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExerciseSubmission>> GetByUserIdAsync(string userId)
    {
        return await _context.ExerciseSubmissions
            .Where(s => s.UserId == userId)
            .Include(s => s.Exercise)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<PerformanceComparison> CreatePerformanceComparisonAsync(PerformanceComparison comparison)
    {
        _context.PerformanceComparisons.Add(comparison);
        await _context.SaveChangesAsync();
        return comparison;
    }
}


