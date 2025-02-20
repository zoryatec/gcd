using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;



public record RunNipkgRequest(string[] arguments, NipkgCmdPath cmd) : IRequest<Result<string,Error>>;

public class RunNipkgHandler(NipkgCmdPath _cmd)
    : IRequestHandler<RunNipkgRequest, Result<string,Error>>
{
    public async Task<Result<string,Error>> Handle(RunNipkgRequest request, CancellationToken cancellationToken)
    {
        var cmd = request.cmd;
        if (cmd == NipkgCmdPath.None) // cmd path from arguments not exist
        {
            cmd = _cmd;
            if (cmd == NipkgCmdPath.None) // cmd path from config not exist
            {
                cmd = NipkgCmdPath.InPath;
                var inPath = TestExists(cmd);
                if(inPath.IsFailure) return Result.Failure<string,Error>(new Error("nipkg not in PATH variable. Please set in PATH, gcd config or pass it as option to cmd"));
            }
        }

        return RunNipkg(cmd, request.arguments);
    }

    private Result<string,Error> TestExists(NipkgCmdPath cmd)
    {
        return RunNipkg(cmd, "--version");
    }

    private Result<string,Error> RunNipkg( NipkgCmdPath cmd,params string[] args)
    {
        //string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";

        var arguments = string.Join(" ", args);


        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = cmd.Value,  
            Arguments = arguments, 
            RedirectStandardOutput = true, 
            RedirectStandardError = true,  
            UseShellExecute = false,     
            CreateNoWindow = false 
        };

        try
        {
            using (Process? process = Process.Start(startInfo))
            {
                _ = process ?? throw new ArgumentNullException(nameof(process));
                // process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(errors))
                {
                    return Result.Failure<string,Error>(new Error(errors));
                }
                return Result.Success<string,Error>(output);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure<string,Error>(new Error(ex.Message));
        }
    }
}


public static class MediatorExtensions
{
    public static async Task<Result<string, Error>> RunNipkgRequestAsync(this IMediator mediator, string[] arguments,
        NipkgCmdPath cmd, CancellationToken cancellationToken = default)
        => await mediator.Send(new RunNipkgRequest(arguments, cmd), cancellationToken);
    public static async Task<Result> AddPackageToLcalFeedAsync(this IMediator mediator, FeedDefinitionLocal feedDefinition, PackageLocalFilePath packagePath, NipkgCmdPath cmd, bool createFeed = false, CancellationToken cancellationToken = default)
    {
        if (createFeed) return await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value, "--create" }, cmd, cancellationToken).MapError(er => er.Message);
        else return await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value }, cmd, cancellationToken).MapError(er => er.Message);
    }


    public static async Task<Result> NipkgPackAsync(this IMediator mediator, BuilderRootDir rootDir, PackageDestinationDirectory destDirectory, NipkgCmdPath cmd, CancellationToken cancellationToken = default) =>
    await mediator.RunNipkgRequestAsync(new string[] { "pack", rootDir.Value, destDirectory.Value }, cmd, cancellationToken).MapError(er => er.Message);
    public static async Task<Result<string,Error>> NipkgGetListInstalledAsync(this IMediator mediator, NipkgCmdPath cmd, CancellationToken cancellationToken = default) =>
        await mediator.RunNipkgRequestAsync(new string[] { "list-installed"}, cmd, cancellationToken);
    
    public static async Task<Result<string,Error>> NipkgGetDependenciesAsync(this IMediator mediator, NipkgCmdPath cmd, string packageName, CancellationToken cancellationToken = default) =>
        await mediator.RunNipkgRequestAsync(new string[] { "get-dependencies",packageName}, cmd, cancellationToken);
}

