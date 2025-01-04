using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;



public record RunNipkgRequest(string[] arguments, NipkgCmdPath cmd) : IRequest<Result>;

public class RunNipkgHandler(NipkgCmdPath _cmd)
    : IRequestHandler<RunNipkgRequest, Result>
{
    public async Task<Result> Handle(RunNipkgRequest request, CancellationToken cancellationToken)
    {
        var cmd = request.cmd;
        if (cmd == NipkgCmdPath.None) // cmd path from arguments not exist
        {
            cmd = _cmd;
            if (cmd == NipkgCmdPath.None) // cmd path from config not exist
            {
                cmd = NipkgCmdPath.InPath;
                var inPath = TestExists(cmd);
                if(inPath.IsFailure) return Result.Failure("nipkg not in PATH variable. Please set in PATH, gcd config or pass it as option to cmd");
            }
        }

        return RunNipkg(cmd, request.arguments);
    }

    private Result TestExists(NipkgCmdPath cmd)
    {
        return RunNipkg(cmd, "--version");
    }

    private Result RunNipkg( NipkgCmdPath cmd,params string[] args)
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
            CreateNoWindow = true 
        };

        try
        {
            using (Process? process = Process.Start(startInfo))
            {
                _ = process ?? throw new ArgumentNullException(nameof(process));
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(errors))
                {
                    return Result.Failure(errors);
                }

            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}


public static class MediatorExtensions
{
    public static async Task<Result> RunNipkgRequestAsync(this IMediator mediator, string[] arguments, NipkgCmdPath cmd, CancellationToken cancellationToken = default)
        => await mediator.Send(new RunNipkgRequest(arguments, cmd), cancellationToken);
    public static async Task<Result> AddPackageToLcalFeedAsync(this IMediator mediator, FeedDefinitionLocal feedDefinition, PackageFilePath packagePath, NipkgCmdPath cmd, bool createFeed = false, CancellationToken cancellationToken = default)
    {
        if (createFeed) return await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value, "--create" }, cmd, cancellationToken);
        else return await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value }, cmd, cancellationToken);
    }


    public static async Task<Result> NipkgPackAsync(this IMediator mediator, BuilderRootDir rootDir, PackageDestinationDirectory destDirectory, NipkgCmdPath cmd, CancellationToken cancellationToken = default) =>
    await mediator.RunNipkgRequestAsync(new string[] { "pack", rootDir.Value, destDirectory.Value }, cmd, cancellationToken);
}

