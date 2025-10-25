using Microsoft.AspNetCore.Mvc;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Interfaces;
using ParallelLab.Core.Services;

namespace ParallelLab.API.Controllers;

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
            return Ok(exercises);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exercises");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> GetExercise(int id)
    {
        try
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id);
            if (exercise == null)
                return NotFound();

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


