using System.Diagnostics;
using System.Text;
using Gcd.SystemProcess.Abstractions;

namespace Gcd.SystemProcess;

public class ProcessService : IProcessService
{
    public async Task<ProcessResponse> ExecuteAsync(ProcessRequest request, CancellationToken cancellationToken = default)
    {
        var arguments = string.Join(" ", request.Arguments);

        var startInfo = new ProcessStartInfo
        {
            FileName = request.ExecutablePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        using var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
                Console.WriteLine(e.Data);
            }
        };
        process.ErrorDataReceived += (s, e) =>
        {
            if (e.Data != null)
            {
                errorBuilder.AppendLine(e.Data);
                Console.WriteLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync(cancellationToken);

        var output = outputBuilder.ToString();
        var errors = errorBuilder.ToString();
        var exitCode = process.ExitCode;

        return new ProcessResponse(exitCode, output, errors);
    }

    public async Task<ProcessResponse> ExecuteAsync(string executablePath, string[] arguments, CancellationToken cancellationToken = default) =>
        await ExecuteAsync(new ProcessRequest(executablePath, arguments), cancellationToken);
    
}


