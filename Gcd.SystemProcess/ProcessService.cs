using System.Diagnostics;
using Gcd.SystemProcess.Abstractions;

namespace Gcd.SystemProcess;

public class ProcessService : IProcessService
{
    public async Task<ProcessResponse> ExecuteAsync(ProcessRequest request, CancellationToken cancellationToken = default)
    {
        var arguments = string.Join(" ", request.Arguments);
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = request.ExecutablePath,  
            Arguments = arguments, 
            RedirectStandardOutput = true, 
            RedirectStandardError = true,  
            UseShellExecute = false,     
            CreateNoWindow = false
        };


        System.Diagnostics.Process? process = System.Diagnostics.Process.Start(startInfo);
        
        _ = process ?? throw new ArgumentNullException(nameof(process));
        process.WaitForExit(new TimeSpan(0, 0, 5));
         // process.WaitForInputIdle(60000);

        string output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        string errors = await process.StandardError.ReadToEndAsync(cancellationToken);
        int  exitCode = process.ExitCode;
        
        return new ProcessResponse(exitCode,output,errors);
        
    }

    public async Task<ProcessResponse> ExecuteAsync(string executablePath, string[] arguments, CancellationToken cancellationToken = default) =>
        await ExecuteAsync(new ProcessRequest(executablePath, arguments), cancellationToken);
    
}


