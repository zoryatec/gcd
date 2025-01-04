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
        if (cmd == NipkgCmdPath.None)
        {
            cmd = _cmd;
            if (cmd == NipkgCmdPath.None) return Result.Failure("Please specify NIPKG path");
        }

        return RunNipkg(request.arguments, cmd);
    }


    private Result RunNipkg(string[] args, NipkgCmdPath cmd)
    {
        //string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";

        var arguments = string.Join(" ", args);


        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = cmd.Value,       // Use "cmd.exe" to run a command
            Arguments = arguments, // "/c" tells cmd to run the command and then terminate
            RedirectStandardOutput = true, // Redirect the output of the command
            RedirectStandardError = true,  // Redirect any errors
            UseShellExecute = false,      // Don't use the shell to execute the command
            CreateNoWindow = true        // Don't create a command window
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


    public static async Task<Result> NipkgPackAsync(this IMediator mediator, PackageBuilderRootDir rootDir, PackageDestinationDirectory destDirectory, NipkgCmdPath cmd, CancellationToken cancellationToken = default) =>
    await mediator.RunNipkgRequestAsync(new string[] { "pack", rootDir.Value, destDirectory.Value }, cmd, cancellationToken);
}

