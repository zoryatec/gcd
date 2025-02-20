using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using MediatR;

namespace Gcd.Handlers.Tools;

public record InstallNinpkgRequest(NipkgInstallerUri InstallerUri, NipkgCmdPath CmdPath) : IRequest<Result>;

public class InstallNinpkgHandler(IMediator _mediator, IFileSystem _fs)
    : IRequestHandler<InstallNinpkgRequest, Result>
{
    public async Task<Result> Handle(InstallNinpkgRequest request, CancellationToken cancellationToken)
    {
        var (installerUri, validationPath) = request;
        var tempDirR = await _fs.GenerateTempDirectoryAsync();
        var tempDir = tempDirR.Value;

        var intallerPath = LocalFilePath.Of($"{tempDir.Value}\\nipkg-installer.exe");


        return await _mediator.DownloadNipkgInstallerAsync(intallerPath.Value, installerUri)
            .Bind(() => InstallAsync(intallerPath.Value));
            //.Bind(() => CheckNipkgVersionAsync(validationPath)); to be implemented once design decision made
    }

    private async Task<Result> CheckNipkgVersionAsync(NipkgCmdPath cmd) =>
        await _mediator.RunNipkgRequestAsync(new string[] { "--version" }, cmd).MapError(err => err.Message);


    static async Task<Result> InstallAsync(LocalFilePath nipkgInstaller)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {

            FileName = nipkgInstaller.Value,         
            Arguments = "--quiet --accept-eulas --prevent-reboot",
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

