using ParallelLab.Core.Entities;

namespace ParallelLab.Core.Services;

public interface ICodeExecutionService
{
    Task<CodeExecutionResult> ExecuteCodeAsync(string code, string testData, int maxExecutionTimeMs);
    Task<CodeExecutionResult> ExecuteIdealSolutionAsync(string idealCode, string testData);
    Task<bool> ValidateCodeAsync(string code);
}

public class CodeExecutionResult
{
    public bool IsSuccess { get; set; }
    public string? Output { get; set; }
    public string? Error { get; set; }
    public long ExecutionTimeMs { get; set; }
    public bool IsCorrect { get; set; }
    public string? CompilationError { get; set; }
    public string? RuntimeError { get; set; }
}


