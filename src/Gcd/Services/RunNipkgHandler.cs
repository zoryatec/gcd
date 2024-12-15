using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Model;
using MediatR;

namespace Gcd.Services;



public record  RunNipkgRequest(string[] arguments) : IRequest<Result>;

public class  RunNipkgHandler(NipkgCmdPath _cmd)
    : IRequestHandler< RunNipkgRequest, Result>
{
    public async Task<Result> Handle( RunNipkgRequest request, CancellationToken cancellationToken)
    {

        return RunNipkg(request.arguments);
    }


    private Result RunNipkg(string[] args)
    {
        //string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";

        var arguments = string.Join(" ", args);


        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = _cmd.Value,       // Use "cmd.exe" to run a command
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
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
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
    public static async Task<Result> RunNipkgRequestAsync(this IMediator mediator, string[] arguments, CancellationToken cancellationToken = default)
        => await mediator.Send(new RunNipkgRequest(arguments), cancellationToken);
    public static async Task<Result> AddPackageToLcalFeedAsync(this IMediator mediator, LocalFeedDefinition feedDefinition, PackageFilePath packagePath, CancellationToken cancellationToken = default) =>
    await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value });
}

