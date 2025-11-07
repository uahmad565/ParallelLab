using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Interfaces;
using ParallelLab.Core.Services;
using System.Security.Claims;

namespace ParallelLab.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseSubmissionRepository _submissionRepository;
    private readonly ICodeExecutionService _codeExecutionService;
    private readonly IPerformanceAnalysisService _performanceAnalysisService;
    private readonly ILogger<ExercisesController> _logger;

    public ExercisesController(
        IExerciseRepository exerciseRepository,
        IExerciseSubmissionRepository submissionRepository,
        ICodeExecutionService codeExecutionService,
        IPerformanceAnalysisService performanceAnalysisService,
        ILogger<ExercisesController> logger)
    {
        _exerciseRepository = exerciseRepository;
        _submissionRepository = submissionRepository;
        _codeExecutionService = codeExecutionService;
        _performanceAnalysisService = performanceAnalysisService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
    {
        try
        {
            var exercises = await _exerciseRepository.GetAllAsync();
            
            // Filter exercises based on user role
            var userRole = GetUserRole();
            
            if (userRole == UserRole.User)
            {
                // Regular users can only see Beginner exercises
                exercises = exercises.Where(e => e.Difficulty == DifficultyLevel.Beginner).ToList();
            }
            // PremiumUser and Admin can see all exercises
            
            return Ok(exercises);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exercises");
            return StatusCode(500, "Internal server error");
        }
    }

    private UserRole GetUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        if (roleClaim == null)
            return UserRole.User; // Default to User if not authenticated

        return Enum.TryParse<UserRole>(roleClaim.Value, out var role) 
            ? role 
            : UserRole.User;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> GetExercise(int id)
    {
        try
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            if (exercise == null)
                return NotFound();

            // Check if user has access to this exercise
            var userRole = GetUserRole();
            
            if (userRole == UserRole.User && exercise.Difficulty != DifficultyLevel.Beginner)
            {
                return StatusCode(403, new { message = "Upgrade to Premium to access this exercise" });
            }

            return Ok(exercise);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exercise {ExerciseId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByCategory(ExerciseCategory category)
    {
        try
        {
            var exercises = await _exerciseRepository.GetByCategoryAsync(category);
            return Ok(exercises);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exercises by category {Category}", category);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("difficulty/{difficulty}")]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByDifficulty(DifficultyLevel difficulty)
    {
        try
        {
            var exercises = await _exerciseRepository.GetByDifficultyAsync(difficulty);
            return Ok(exercises);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exercises by difficulty {Difficulty}", difficulty);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Exercise>> CreateExercise([FromBody] Exercise exercise)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdExercise = await _exerciseRepository.CreateAsync(exercise);
            return CreatedAtAction(nameof(GetExercise), new { id = createdExercise.Id }, createdExercise);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating exercise");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExercise(int id, [FromBody] Exercise exercise)
    {
        try
        {
            if (id != exercise.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _exerciseRepository.UpdateAsync(exercise);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating exercise {ExerciseId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        try
        {
            await _exerciseRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting exercise {ExerciseId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}


