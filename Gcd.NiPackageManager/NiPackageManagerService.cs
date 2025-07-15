using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.NiPackageManager.Abstractions;
using Gcd.SystemProcess.Abstractions;

namespace Gcd.NiPackageManager;

public class NiPackageManagerService(IProcessService _processService)  : INiPackageManagerService
{
    private string _cmd = @"C:\Program Files\National Instruments\NI Package Manager\nipkg.exe";
    public async Task<Result> Install(InstallRequest request)
    {
        var arguments = new List<string>();
        var fullSpecPackages = new List<string>();
        foreach (var packageToInstall in request.PackagesToInstalls)
        {
            fullSpecPackages.Add($"{packageToInstall.Package}={packageToInstall.Version}");
        }
        // var packages = string.Join(" ", fullSpecPackages);
        
        arguments.Add("install");
        arguments.AddRange(fullSpecPackages);
        if( request.AcceptEulas) { arguments.Add("--accept-eulas"); }
        if( request.AssumeYes) { arguments.Add("--assume-yes"); }
        if( request.Simulate) { arguments.Add("--simulate"); }
        if( request.ForceLocked) { arguments.Add("--force-locked"); }

        return await RunCommand(arguments.ToArray());
    }
    
    public async Task<Result> VersionAsync()
    {
        return await RunCommand("--version");
    }
    
    private async Task<Result> RunCommand( params string[] args)
    {
        var response =  await _processService.ExecuteAsync(_cmd, args.ToArray());
        if ( response.ExitCode != 0) { return Result.Failure(response.StandardOutput + response.StandardError); }
        
        return Result.Success(response.StandardOutput); 
         
    }

}
