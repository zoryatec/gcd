using System.Diagnostics;
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

        using var process = System.Diagnostics.Process.Start(startInfo)
                            ?? throw new ArgumentNullException("nameof(process)");

        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        var output = await outputTask;
        var errors = await errorTask;
        var exitCode = process.ExitCode;

        return new ProcessResponse(exitCode, output, errors);
    }

    public async Task<ProcessResponse> ExecuteAsync(string executablePath, string[] arguments, CancellationToken cancellationToken = default) =>
        await ExecuteAsync(new ProcessRequest(executablePath, arguments), cancellationToken);
    
}


