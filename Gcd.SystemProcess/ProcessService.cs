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


        using (System.Diagnostics.Process? process = System.Diagnostics.Process.Start(startInfo))
        {
            _ = process ?? throw new ArgumentNullException(nameof(process));
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            int  exitCode = process.ExitCode;
            
            return new ProcessResponse(exitCode,output,errors);
        }
    }

    public async Task<ProcessResponse> ExecuteAsync(string executablePath, string[] arguments, CancellationToken cancellationToken = default) =>
        await ExecuteAsync(new ProcessRequest(executablePath, arguments), cancellationToken);
    
}


