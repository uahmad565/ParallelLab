using Microsoft.EntityFrameworkCore;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Interfaces;
using ParallelLab.Infrastructure.Data;

namespace ParallelLab.Infrastructure.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ParallelLabDbContext _context;

    public ExerciseRepository(ParallelLabDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await _context.Exercises
            .Where(e => e.IsActive)
            .OrderBy(e => e.Category)
            .ThenBy(e => e.Difficulty)
            .ToListAsync();
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        return await _context.Exercises
            .Include(e => e.Submissions)
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
    }

    public async Task<Exercise> CreateAsync(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();
        return exercise;
    }

    public async Task<Exercise> UpdateAsync(Exercise exercise)
    {
        exercise.UpdatedAt = DateTime.UtcNow;
        _context.Exercises.Update(exercise);
        await _context.SaveChangesAsync();
        return exercise;
    }

    public async Task DeleteAsync(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise != null)
        {
            exercise.IsActive = false;
            exercise.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Exercise>> GetByCategoryAsync(ExerciseCategory category)
    {
        return await _context.Exercises
            .Where(e => e.Category == category && e.IsActive)
            .OrderBy(e => e.Difficulty)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty)
    {
        return await _context.Exercises
            .Where(e => e.Difficulty == difficulty && e.IsActive)
            .OrderBy(e => e.Category)
            .ToListAsync();
    }
}


