namespace RunC_CodeFiles.Models
{
    public class CodeRunResponse
    {
        public int ExitCode { get; set; }
        public bool TimedOut { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public string StandardOutput { get; set; } = string.Empty;
        public string StandardError { get; set; } = string.Empty;
    }
}


