using ParallelLab.Core.Entities;

namespace ParallelLab.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
}

public interface IExerciseRepository
{
    Task<IEnumerable<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(int id);
    Task<Exercise> CreateAsync(Exercise exercise);
    Task<Exercise> UpdateAsync(Exercise exercise);
    Task DeleteAsync(int id);
    Task<IEnumerable<Exercise>> GetByCategoryAsync(ExerciseCategory category);
    Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty);
}

public interface IExerciseSubmissionRepository
{
    Task<ExerciseSubmission> CreateAsync(ExerciseSubmission submission);
    Task<ExerciseSubmission> UpdateAsync(ExerciseSubmission submission);
    Task<ExerciseSubmission?> GetByIdAsync(int id);
    Task<IEnumerable<ExerciseSubmission>> GetByExerciseIdAsync(int exerciseId);
    Task<IEnumerable<ExerciseSubmission>> GetByUserIdAsync(string userId);
    Task<PerformanceComparison> CreatePerformanceComparisonAsync(PerformanceComparison comparison);
    Task<TestCaseResult> CreateTestCaseResultAsync(TestCaseResult result);
}


