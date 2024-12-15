using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadNipkg;
using Gcd.Commands.SystemAddToPath;
using Gcd.Model;
using Gcd.Services;
using Gcd.Tests.EndToEnd;
using MediatR;

namespace Gcd.Commands.NipkgInstallNipkg;

public record InstallNinpkgRequest() : IRequest<Result>;
public record InstallNinpkgResponse(string result);

public class InstallNinpkgHandler(IMediator _mediator, ITempDirectoryProvider _tempDir)
    : IRequestHandler<InstallNinpkgRequest, Result>
{
    public async Task<Result> Handle(InstallNinpkgRequest request, CancellationToken cancellationToken)
    {
        var tempDirR = await _tempDir.GenerateTempDirectoryAsync();
        var tempDir = tempDirR.Value;

        var intallerPath = LocalFilePath.Offf($"{tempDir.Value}\\nipkg-installer.exe");

        return await _mediator.DownloadNipkgInstallerAsync(intallerPath.Value)
            .Bind(() => InstallAsync(intallerPath.Value))
            .Bind(() => CheckNipkgVersionAsync());
    }

    private async Task<Result> CheckNipkgVersionAsync() =>
        await _mediator.RunNipkgRequestAsync(new string[] { "--version" });


    static async Task<Result> InstallAsync(LocalFilePath nipkgInstaller)
    {
        // Initialize the ProcessStartInfo with the command and arguments
        ProcessStartInfo startInfo = new ProcessStartInfo
        {

            FileName = nipkgInstaller.Value,         // Use "cmd.exe" to run a command
            Arguments = "--quiet --accept-eulas --prevent-reboot",
            RedirectStandardOutput = true,  // Redirect the output of the command
            RedirectStandardError = true,   // Redirect any errors
            UseShellExecute = false,       // Don't use the shell to execute the command
            CreateNoWindow = false         // Don't create a command 
        };

        try
        {
            using (Process? process = Process.Start(startInfo))
            {
                _ = process ?? throw new ArgumentNullException(nameof(process));
                // Read the standard output and error


                // Wait for the command to finish
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

            return Result.Failure(ex.Message); ;
        }
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> InstallNipkgInstallerAsync(this IMediator mediator, CancellationToken cancellationToken = default)
        => await mediator.Send(new InstallNinpkgRequest(), cancellationToken);
}
