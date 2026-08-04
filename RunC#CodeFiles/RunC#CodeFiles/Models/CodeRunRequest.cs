namespace RunC_CodeFiles.Models
{
    public class CodeRunRequest
    {
        public string? Code { get; set; }
        public string? Input { get; set; }
        public int? TimeoutMs { get; set; }
    }
}


