using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RunC_CodeFiles.Models;

namespace RunC_CodeFiles.Services
{
    public static class CodeRunner
    {
        public static async Task<CodeRunResponse> RunAsync(string sourceCode, string input, int timeoutMs)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), $"submission_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDir);
            string exePath = Path.Combine(tempDir, "submission.exe");
            string runtimeConfigPath = Path.Combine(tempDir, "submission.runtimeconfig.json");

            var compileOk = CompileSourceToExe(sourceCode, exePath, out var diagnostics);
            if (!compileOk)
            {
                try { Directory.Delete(tempDir, true); } catch { }
                return new CodeRunResponse
                {
                    ExitCode = 4,
                    TimedOut = false,
                    ElapsedMilliseconds = 0,
                    StandardOutput = string.Empty,
                    StandardError = string.Join("\n", diagnostics.Select(d => d.ToString()))
                };
            }

            CreateRuntimeConfig(runtimeConfigPath);

            var result = await RunProcessWithInput(exePath, input, timeoutMs, tempDir);

            try { Directory.Delete(tempDir, true); } catch { }

            return new CodeRunResponse
            {
                ExitCode = result.ExitCode,
                TimedOut = result.TimedOut,
                ElapsedMilliseconds = result.ElapsedMilliseconds,
                StandardOutput = result.StandardOutput,
                StandardError = result.StandardError
            };
        }

        private static bool CompileSourceToExe(string sourceCode, string outputExePath, out IEnumerable<Diagnostic> diagnostics)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var refs = new List<MetadataReference>();
            var assemblies = new[]
            {
                typeof(object).GetTypeInfo().Assembly,
                typeof(Console).GetTypeInfo().Assembly,
                typeof(Enumerable).GetTypeInfo().Assembly,
                typeof(List<>).GetTypeInfo().Assembly,
                typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly
            }.Distinct();

            foreach (var asm in assemblies)
            {
                if (asm?.Location != null)
                    refs.Add(MetadataReference.CreateFromFile(asm.Location));
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (!string.IsNullOrEmpty(asm.Location))
                        refs.Add(MetadataReference.CreateFromFile(asm.Location));
                }
                catch { }
            }

            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(outputExePath),
                new[] { syntaxTree },
                refs,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                    .WithOptimizationLevel(OptimizationLevel.Release)
            );

            using (var fs = new FileStream(outputExePath, FileMode.Create, FileAccess.Write))
            {
                var result = compilation.Emit(fs);
                diagnostics = result.Diagnostics;
                return result.Success;
            }
        }

        private static void CreateRuntimeConfig(string configPath)
        {
            var config = @"{
  ""runtimeOptions"": {
    ""tfm"": ""net8.0"",
    ""framework"": {
      ""name"": ""Microsoft.NETCore.App"",
      ""version"": ""8.0.0""
    }
  }
}";
            File.WriteAllText(configPath, config);
        }

        private class RunResult
        {
            public int ExitCode { get; set; }
            public bool TimedOut { get; set; }
            public long ElapsedMilliseconds { get; set; }
            public string StandardOutput { get; set; } = string.Empty;
            public string StandardError { get; set; } = string.Empty;
        }

        private static async Task<RunResult> RunProcessWithInput(string exePath, string input, int timeoutMs, string tempDir)
        {
            var result = new RunResult();
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{exePath}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            psi.Environment["OUTPUT_PATH"] = Path.Combine(tempDir, "output.txt");

            using (var proc = new Process { StartInfo = psi })
            {
                var sw = new Stopwatch();
                proc.Start();

                var writeInputTask = Task.Run(async () =>
                {
                    using (var stdin = proc.StandardInput)
                    {
                        if (!string.IsNullOrEmpty(input))
                        {
                            await stdin.WriteAsync(input);
                            if (!input.EndsWith(Environment.NewLine)) await stdin.WriteAsync(Environment.NewLine);
                        }
                        stdin.Close();
                    }
                });

                sw.Start();

                var readOutTask = proc.StandardOutput.ReadToEndAsync();
                var readErrTask = proc.StandardError.ReadToEndAsync();

                var processExited = await Task.Run(() => proc.WaitForExit(timeoutMs));
                if (!processExited)
                {
                    try { proc.Kill(true); } catch { }
                    result.TimedOut = true;
                }
                else
                {
                    result.TimedOut = false;
                }

                sw.Stop();
                result.ElapsedMilliseconds = sw.ElapsedMilliseconds;
                result.StandardOutput = await readOutTask;
                result.StandardError = await readErrTask;
                try { result.ExitCode = proc.HasExited ? proc.ExitCode : -1; } catch { result.ExitCode = -1; }
            }

            return result;
        }
    }
}


