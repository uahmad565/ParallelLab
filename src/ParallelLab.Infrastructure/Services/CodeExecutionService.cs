using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;
using ParallelLab.Core.Services;

namespace ParallelLab.Infrastructure.Services;

public class CodeExecutionService : ICodeExecutionService
{
    private readonly ILogger<CodeExecutionService> _logger;

    public CodeExecutionService(ILogger<CodeExecutionService> logger)
    {
        _logger = logger;
    }

    public async Task<CodeExecutionResult> ExecuteCodeAsync(string code, string testData, int maxExecutionTimeMs)
    {
        try
        {
            // First validate the code
            var isValid = await ValidateCodeAsync(code);
            if (!isValid)
            {
                return new CodeExecutionResult
                {
                    IsSuccess = false,
                    CompilationError = "Code validation failed - contains potentially dangerous operations"
                };
            }

            // Compile the code
            var compilationResult = CompileCode(code);
            if (!compilationResult.IsSuccess)
            {
                return new CodeExecutionResult
                {
                    IsSuccess = false,
                    CompilationError = compilationResult.Error
                };
            }

            // Execute the code with timeout
            var executionResult = await ExecuteWithTimeoutAsync(compilationResult.Assembly!, testData, maxExecutionTimeMs);
            
            return executionResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing user code");
            return new CodeExecutionResult
            {
                IsSuccess = false,
                RuntimeError = ex.Message
            };
        }
    }

    public async Task<CodeExecutionResult> ExecuteIdealSolutionAsync(string idealCode, string testData)
    {
        return await ExecuteCodeAsync(idealCode, testData, 30000); // 30 second timeout for ideal solutions
    }

    public async Task<bool> ValidateCodeAsync(string code)
    {
        // Check for potentially dangerous operations
        var dangerousPatterns = new[]
        {
            "System.IO.File",
            "System.IO.Directory",
            "System.Net",
            "System.Diagnostics.Process",
            "System.Reflection",
            "System.Security",
            "System.Environment",
            "Console.Read",
            "Console.ReadLine"
        };

        var upperCode = code.ToUpperInvariant();
        return await Task.FromResult(!dangerousPatterns.Any(pattern => upperCode.Contains(pattern.ToUpperInvariant())));
    }

    private CompilationResult CompileCode(string code)
    {
        try
        {
            // Create a wrapper for the user code
            var wrappedCode = CreateExecutionWrapper(code);
            
            var syntaxTree = CSharpSyntaxTree.ParseText(wrappedCode);
            
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Concurrent.ConcurrentBag<>).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                "UserCodeAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics.Select(d => d.ToString()));
                return new CompilationResult { IsSuccess = false, Error = errors };
            }

            ms.Seek(0, SeekOrigin.Begin);
            return new CompilationResult { IsSuccess = true, Assembly = ms.ToArray() };
        }
        catch (Exception ex)
        {
            return new CompilationResult { IsSuccess = false, Error = ex.Message };
        }
    }

    private string CreateExecutionWrapper(string userCode)
    {
        // Remove access modifiers from user code to prevent compilation errors
        var cleanedCode = CleanUserCode(userCode);
        
        return $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

public class UserCodeExecutor
{{
    public static string Execute(string testData)
    {{
        var output = new StringBuilder();
        
        try
        {{
            // User's code goes here
            {cleanedCode}
            
            return output.ToString();
        }}
        catch (Exception ex)
        {{
            return $""Error: {{ex.Message}}"";
        }}
    }}
}}";
    }

    private string CleanUserCode(string userCode)
    {
        // Remove access modifiers that would cause compilation errors when placed inside a method
        var lines = userCode.Split('\n');
        var cleanedLines = new List<string>();
        
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                cleanedLines.Add(line);
                continue;
            }
            
            // Remove public, private, protected, internal modifiers
            if (trimmedLine.StartsWith("public ") || 
                trimmedLine.StartsWith("private ") || 
                trimmedLine.StartsWith("protected ") || 
                trimmedLine.StartsWith("internal "))
            {
                // Remove the access modifier but keep the rest of the line
                var withoutModifier = trimmedLine.Substring(trimmedLine.IndexOf(' ') + 1);
                cleanedLines.Add(line.Replace(trimmedLine, withoutModifier));
            }
            else
            {
                cleanedLines.Add(line);
            }
        }
        
        return string.Join("\n", cleanedLines);
    }

    private async Task<CodeExecutionResult> ExecuteWithTimeoutAsync(byte[] assembly, string testData, int maxExecutionTimeMs)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(maxExecutionTimeMs));
            
            var task = Task.Run(() =>
            {
                var loadedAssembly = System.Reflection.Assembly.Load(assembly);
                var type = loadedAssembly.GetType("UserCodeExecutor");
                var method = type?.GetMethod("Execute");
                
                if (method == null)
                    throw new InvalidOperationException("Could not find Execute method in user code");
                
                return method.Invoke(null, new object[] { testData })?.ToString() ?? "";
            }, cts.Token);

            var result = await task;
            stopwatch.Stop();

            return new CodeExecutionResult
            {
                IsSuccess = true,
                Output = result,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                IsCorrect = true // This would be determined by comparing with expected output
            };
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            return new CodeExecutionResult
            {
                IsSuccess = false,
                RuntimeError = $"Execution timed out after {maxExecutionTimeMs}ms",
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new CodeExecutionResult
            {
                IsSuccess = false,
                RuntimeError = ex.Message,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
    }

    private class CompilationResult
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public byte[]? Assembly { get; set; }
    }
}


