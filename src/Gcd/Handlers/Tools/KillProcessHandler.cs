using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Tools;

public record KillProcessRequest(ProcessName ProcessName) : IRequest<UnitResult<Error>>;

public class KillProcessHandler()
    : IRequestHandler<KillProcessRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(KillProcessRequest request, CancellationToken cancellationToken)
    {
        var processName = request.ProcessName;
        Process[] processes = Process.GetProcessesByName(processName.Value);
        
        foreach (var process in processes)
        {
            try
            {
                process.Kill(); 
                return UnitResult.Success<Error>();
            }
            catch (Exception ex)
            {
               UnitResult.Failure<Error>(new Error($"Failed to kill process '{process.ProcessName}'. {ex.Message}"));
            }
        }

        return UnitResult.Success<Error>();
    }
}

public record ProcessName
{
    public static ProcessName LabView => new ProcessName("LabVIEW");
    public static ProcessName Vipm => new ProcessName("VI Package Manager");
    private ProcessName(string value) => Value = value;
    public string Value { get; }
}


