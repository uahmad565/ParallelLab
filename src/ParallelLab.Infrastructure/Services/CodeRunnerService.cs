using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParallelLab.Core.Services;

namespace ParallelLab.Infrastructure.Services;

public class CodeRunnerService : ICodeRunnerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CodeRunnerService> _logger;
    private readonly string _codeRunnerUrl;

    public CodeRunnerService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<CodeRunnerService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _codeRunnerUrl = configuration["CodeRunner:Url"] ?? "http://localhost:8080";
    }

    public async Task<CodeRunnerResponse> RunCodeAsync(string code, string input, int timeoutMs)
    {
        try
        {
            var request = new CodeRunnerRequest
            {
                Code = code,
                Input = input,
                TimeoutMs = timeoutMs
            };

            var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending code execution request to Code Runner API: {Url}", _codeRunnerUrl);

            var response = await _httpClient.PostAsync($"{_codeRunnerUrl}/run", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Code Runner API returned error: {StatusCode} - {Error}", 
                    response.StatusCode, errorContent);
                
                return new CodeRunnerResponse
                {
                    ExitCode = -1,
                    TimedOut = false,
                    ElapsedMilliseconds = 0,
                    StandardOutput = string.Empty,
                    StandardError = $"Code Runner API error: {response.StatusCode}"
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CodeRunnerResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return result ?? new CodeRunnerResponse
            {
                ExitCode = -1,
                StandardError = "Failed to parse Code Runner response"
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error calling Code Runner API");
            return new CodeRunnerResponse
            {
                ExitCode = -1,
                TimedOut = false,
                ElapsedMilliseconds = 0,
                StandardOutput = string.Empty,
                StandardError = $"Failed to connect to Code Runner API: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Code Runner API");
            return new CodeRunnerResponse
            {
                ExitCode = -1,
                TimedOut = false,
                ElapsedMilliseconds = 0,
                StandardOutput = string.Empty,
                StandardError = $"Error: {ex.Message}"
            };
        }
    }
}

