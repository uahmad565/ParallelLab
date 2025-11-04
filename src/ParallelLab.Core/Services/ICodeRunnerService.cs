namespace ParallelLab.Core.Services;

public interface ICodeRunnerService
{
    Task<CodeRunnerResponse> RunCodeAsync(string code, string input, int timeoutMs);
}

public class CodeRunnerRequest
{
    public string Code { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public int TimeoutMs { get; set; } = 5000;
}

public class CodeRunnerResponse
{
    public int ExitCode { get; set; }
    public bool TimedOut { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public string StandardOutput { get; set; } = string.Empty;
    public string StandardError { get; set; } = string.Empty;
}

