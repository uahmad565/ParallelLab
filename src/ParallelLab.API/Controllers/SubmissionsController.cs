using Microsoft.AspNetCore.Mvc;
using ParallelLab.Core.Entities;
using ParallelLab.Core.Interfaces;
using ParallelLab.Core.Services;

namespace ParallelLab.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseSubmissionRepository _submissionRepository;
    private readonly ICodeExecutionService _codeExecutionService;
    private readonly IPerformanceAnalysisService _performanceAnalysisService;
    private readonly ILogger<SubmissionsController> _logger;

    public SubmissionsController(
        IExerciseRepository exerciseRepository,
        IExerciseSubmissionRepository submissionRepository,
        ICodeExecutionService codeExecutionService,
        IPerformanceAnalysisService performanceAnalysisService,
        ILogger<SubmissionsController> logger)
    {
        _exerciseRepository = exerciseRepository;
        _submissionRepository = submissionRepository;
        _codeExecutionService = codeExecutionService;
        _performanceAnalysisService = performanceAnalysisService;
        _logger = logger;
    }

    [HttpPost("submit")]
    public async Task<ActionResult<ExerciseSubmission>> SubmitCode([FromBody] CodeSubmissionRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the exercise
            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId);
            if (exercise == null)
                return NotFound("Exercise not found");

            // Execute the user's code
            var executionResult = await _codeExecutionService.ExecuteCodeAsync(
                request.UserCode, 
                exercise.TestData, 
                exercise.MaxExecutionTimeMs);

            // Create submission record
            var submission = new ExerciseSubmission
            {
                ExerciseId = request.ExerciseId,
                UserCode = request.UserCode,
                CompilationError = executionResult.CompilationError,
                RuntimeError = executionResult.RuntimeError,
                ExecutionTimeMs = executionResult.ExecutionTimeMs,
                IsCorrect = executionResult.IsCorrect,
                Output = executionResult.Output,
                UserId = request.UserId ?? "anonymous",
                Exercise = exercise // Set the exercise navigation property
            };

            // Save the submission first to get the ID
            var createdSubmission = await _submissionRepository.CreateAsync(submission);

            // If execution was successful, perform performance analysis
            if (executionResult.IsSuccess)
            {
                var performanceAnalysis = await _performanceAnalysisService.AnalyzePerformanceAsync(
                    createdSubmission, 
                    exercise.ExpectedExecutionTimeMs);

                createdSubmission.PerformanceScore = performanceAnalysis.PerformanceScore;

                // Create performance comparison with the saved submission
                var comparison = await _performanceAnalysisService.CompareWithIdealAsync(
                    createdSubmission, 
                    exercise.IdealSolution, 
                    exercise.TestData);

                await _submissionRepository.CreatePerformanceComparisonAsync(comparison);
            }

            return Ok(new CodeSubmissionResponse
            {
                Submission = createdSubmission,
                ExecutionResult = executionResult,
                PerformanceAnalysis = executionResult.IsSuccess ? 
                    await _performanceAnalysisService.AnalyzePerformanceAsync(createdSubmission, exercise.ExpectedExecutionTimeMs) : 
                    null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting code for exercise {ExerciseId}", request.ExerciseId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("exercise/{exerciseId}")]
    public async Task<ActionResult<IEnumerable<ExerciseSubmission>>> GetSubmissionsByExercise(int exerciseId)
    {
        try
        {
            var submissions = await _submissionRepository.GetByExerciseIdAsync(exerciseId);
            return Ok(submissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving submissions for exercise {ExerciseId}", exerciseId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ExerciseSubmission>>> GetSubmissionsByUser(string userId)
    {
        try
        {
            var submissions = await _submissionRepository.GetByUserIdAsync(userId);
            return Ok(submissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving submissions for user {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseSubmission>> GetSubmission(int id)
    {
        try
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission == null)
                return NotFound();

            return Ok(submission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving submission {SubmissionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}

public class CodeSubmissionRequest
{
    public int ExerciseId { get; set; }
    public string UserCode { get; set; } = string.Empty;
    public string? UserId { get; set; }
}

public class CodeSubmissionResponse
{
    public ExerciseSubmission Submission { get; set; } = null!;
    public CodeExecutionResult ExecutionResult { get; set; } = null!;
    public PerformanceAnalysisResult? PerformanceAnalysis { get; set; }
}


