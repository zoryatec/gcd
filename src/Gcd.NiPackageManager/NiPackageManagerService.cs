using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.NiPackageManager.Abstractions;
using Gcd.SystemProcess.Abstractions;

namespace Gcd.NiPackageManager;

public class NiPackageManagerService(IProcessService _processService)  : INiPackageManagerService
{
    private string _cmd = @"C:\Program Files\National Instruments\NI Package Manager\nipkg.exe";
    public async Task<Result> InstallAsync(InstallRequest request)
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
        if( request.AllowDowngrade) { arguments.Add("--allow-downgrade"); }
        if( request.AllowUninstall) { arguments.Add("--allow-uninstall"); }
        if( request.InstallAlsoUpgrades) { arguments.Add("--install-also-upgrades"); }
        if( request.IncludeRecommended) { arguments.Add("--include-recommended"); }

        var result = await RunCommand(arguments.ToArray());
        var value = result.Value;
        return await new OutputParser().ParseGeneric(value); 
    }

    public async Task<Result> RemoveAsync(RemoveRequest request)
    {
        var arguments = new List<string>();
        var fullSpecPackages = new List<string>();
        foreach (var packageToInstall in request.PackagesToRemove)
        {
            var fullspec = $"{packageToInstall.Package}={packageToInstall.Version}";
            if(string.IsNullOrWhiteSpace(packageToInstall.Version)) {fullspec = packageToInstall.Package;}
            fullSpecPackages.Add(fullspec);
        }
        // var packages = string.Join(" ", fullSpecPackages);
        
        arguments.Add("remove");
        arguments.AddRange(fullSpecPackages);
        if( request.AssumeYes) { arguments.Add("--assume-yes"); }
        if( request.Simulate) { arguments.Add("--simulate"); }
        if( request.ForceLocked) { arguments.Add("--force-locked"); }

        var result = await RunCommand(arguments.ToArray());
        var value = result.Value;
        return await new OutputParser().ParseGeneric(value); 
;
    }

    public async Task<Result<InfoInstalledResponse>> InfoInstalledAsync(InfoInstalledRequest request)
    {
        var arguments = new List<string>();
        arguments.Add("info-installed");
        arguments.Add(request.Pattern);
        var response = await RunCommand(arguments.ToArray());
        var value = response.Value;

        return await new OutputParser().ParseInfoInstalledAsync(value);    
    }

    public async Task<Result<string>> AddFeedAsync(AddFeedRequest request)
    {
         var (name , uri) = request.FeedToAdd;
        
            var arguments = new List<string>();
            arguments.Add("feed-add");
            arguments.Add($"--name={name}");
            arguments.Add("--system");
            arguments.Add(uri);
            
            var response = await RunCommand(arguments.ToArray());
            var value = response.Value;
            return await new OutputParser().ParseGeneric(value); 
    }

    public async Task<Result<string>> UpdateAsync()
    {
        var arguments = new List<string>();
        arguments.Add("update");
        var response = await RunCommand(arguments.ToArray());
        var value = response.Value;

        return await new OutputParser().ParseGeneric(value);    
    }

    public async Task<Result<string>> RemoveFeedAsync(RemoveFeedsRequest request)
    {
        var (name , uri) = request.FeedToRemove;
        
        var arguments = new List<string>();
        arguments.Add("feed-remove");
        arguments.Add(name);
            
        var response = await RunCommand(arguments.ToArray());
        var value = response.Value;
        return await new OutputParser().ParseGeneric(value); 
    }

    public async Task<Result> VersionAsync()
    {
        return await RunCommand("--version");
    }
    
    private async Task<Result<NiPackageManagerOutput>> RunCommand( params string[] args)
    {
        var response =  await _processService.ExecuteAsync(_cmd, args.ToArray());
        
        return Result.Success<NiPackageManagerOutput>(new NiPackageManagerOutput(response.ExitCode,
            response.StandardOutput, response.StandardError)); 
    }

}
