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
    private readonly ICodeRunnerService _codeRunnerService;
    private readonly IPerformanceAnalysisService _performanceAnalysisService;
    private readonly ILogger<SubmissionsController> _logger;

    public SubmissionsController(
        IExerciseRepository exerciseRepository,
        IExerciseSubmissionRepository submissionRepository,
        ICodeExecutionService codeExecutionService,
        ICodeRunnerService codeRunnerService,
        IPerformanceAnalysisService performanceAnalysisService,
        ILogger<SubmissionsController> logger)
    {
        _exerciseRepository = exerciseRepository;
        _submissionRepository = submissionRepository;
        _codeExecutionService = codeExecutionService;
        _codeRunnerService = codeRunnerService;
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

            // Get the exercise with test cases
            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId);
            if (exercise == null)
                return NotFound("Exercise not found");

            if (exercise.TestCases == null || !exercise.TestCases.Any())
                return BadRequest("Exercise has no test cases defined");

            // Create submission record
            var submission = new ExerciseSubmission
            {
                ExerciseId = request.ExerciseId,
                UserCode = request.UserCode,
                UserId = request.UserId ?? "anonymous",
                Exercise = exercise
            };

            // Save the submission first to get the ID
            var createdSubmission = await _submissionRepository.CreateAsync(submission);

            // Run code against all test cases
            var testCaseResults = new List<TestCaseResult>();
            var allPassed = true;
            long totalExecutionTime = 0;
            string? compilationError = null;

            foreach (var testCase in exercise.TestCases.OrderBy(tc => tc.Order))
            {
                _logger.LogInformation("Running test case {TestCaseId} for submission {SubmissionId}", 
                    testCase.Id, createdSubmission.Id);

                var runnerResponse = await _codeRunnerService.RunCodeAsync(
                    request.UserCode, 
                    testCase.Input, 
                    testCase.TimeoutMs);

                var actualOutput = runnerResponse.StandardOutput.TrimEnd('\r', '\n');
                var expectedOutput = testCase.ExpectedOutput.TrimEnd('\r', '\n');
                var passed = runnerResponse.ExitCode == 0 && 
                            !runnerResponse.TimedOut && 
                            actualOutput == expectedOutput;
            
                if (!passed)
                    allPassed = false;

                if (!string.IsNullOrEmpty(runnerResponse.StandardError) && runnerResponse.ExitCode != 0)
                {
                    compilationError = runnerResponse.StandardError;
            }

                var testCaseResult = new TestCaseResult
                {
                    SubmissionId = createdSubmission.Id,
                    TestCaseId = testCase.Id,
                    Passed = passed,
                    ActualOutput = actualOutput,
                    ExpectedOutput = expectedOutput,
                    ExecutionTimeMs = runnerResponse.ElapsedMilliseconds,
                    TimedOut = runnerResponse.TimedOut,
                    ExitCode = runnerResponse.ExitCode,
                    StandardError = runnerResponse.StandardError
                };

                testCaseResults.Add(testCaseResult);
                totalExecutionTime += runnerResponse.ElapsedMilliseconds;
            }

            // Update submission with results
            createdSubmission.IsCorrect = allPassed;
            createdSubmission.ExecutionTimeMs = totalExecutionTime / exercise.TestCases.Count; // Average execution time
            createdSubmission.CompilationError = compilationError;
            createdSubmission.Output = string.Join("\n", testCaseResults.Select(r => 
                $"Test {r.TestCaseId}: {(r.Passed ? "PASSED" : "FAILED")} ({r.ExecutionTimeMs}ms)"));

            // Calculate performance score
            if (allPassed)
            {
                var avgIdealTime = exercise.TestCases.Average(tc => tc.IdealExecutionTimeMs);
                var performanceRatio = (double)avgIdealTime / createdSubmission.ExecutionTimeMs;
                createdSubmission.PerformanceScore = Math.Min(100, performanceRatio * 100);
            }

            // Save test case results
            foreach (var result in testCaseResults)
        {
                await _submissionRepository.CreateTestCaseResultAsync(result);
            }

            await _submissionRepository.UpdateAsync(createdSubmission);

            return Ok(new CodeSubmissionResponse
            {
                Submission = createdSubmission,
                TestCaseResults = testCaseResults,
                TotalTests = testCaseResults.Count,
                PassedTests = testCaseResults.Count(r => r.Passed)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting code for exercise {ExerciseId}", request.ExerciseId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
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
    public List<TestCaseResult> TestCaseResults { get; set; } = new();
    public int TotalTests { get; set; }
    public int PassedTests { get; set; }
}


