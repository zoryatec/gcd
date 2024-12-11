using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Model;
using Gcd.Services;
using MediatR;
using System.Reflection.Metadata;

namespace Gcd.Commands.NipkgPackageBuild;

public record PackageBuildRequest(PackageContentSourceDir PackageContentPath, PackageName PackageName, PackageVersion PackageVersion, PackageInstalationDir PackageInstalationDir, PackageDestinationDirectory PackageDestinationDir) : IRequest<Result>;


public class PackageBuildHandler(IMediator _mediator)
    : IRequestHandler<PackageBuildRequest, Result>
{
    public async Task<Result> Handle(PackageBuildRequest request, CancellationToken cancellationToken)
    {

        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string pckgDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        if (!Directory.Exists(pckgDirectory)) Directory.CreateDirectory(pckgDirectory);

        var pckBuilderDest = LocalDirPath.Of(temporaryDirectory);
        var pckDefinitionRes = PackageBuilderDefinition.Of(pckBuilderDest.Value, request.PackageInstalationDir);
        var pckDefiniton = pckDefinitionRes.Value;

        var temporaryDir = PackageContentSourceDir.Create(temporaryDirectory);

        var subRequest = await _mediator.PackageBuilderInitAsync(temporaryDir.Value, request.PackageName, request.PackageVersion, request.PackageInstalationDir);


        CopyDirectoryContents(request.PackageContentPath.Value, pckDefiniton.ContentDir.Value);

        var result = RunCommand(temporaryDirectory, pckgDirectory);
        string packageFileName = $"{request.PackageName.Value}_{request.PackageVersion.Value}_windows_x64.nipkg";
        string packageFilePath = Path.Combine(pckgDirectory, packageFileName);

        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDestinationDir = Path.Combine(currentDirectoryPath, request.PackageDestinationDir.Value);

        if (!Directory.Exists(packageDestinationDir)) Directory.CreateDirectory(packageDestinationDir);

        string packageDestinationFilePath = Path.Combine(packageDestinationDir, packageFileName);
        File.Copy(packageFilePath, packageDestinationFilePath, overwrite: true);

        Directory.Delete(temporaryDirectory, true);
        Directory.Delete(pckgDirectory, true);

        return Result.Success();
    }

    static void CopyDirectoryContents(string sourceDir, string destinationDir)
    {
        // Ensure the source directory exists
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDir}");
        }

        // Create the destination directory if it does not exist
        Directory.CreateDirectory(destinationDir);

        // Copy all files from source to destination
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destinationDir, fileName);
            File.Copy(file, destFile, overwrite: true);
        }

        // Recursively copy all subdirectories
        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            string directoryName = Path.GetFileName(directory);
            string destDir = Path.Combine(destinationDir, directoryName);
            CopyDirectoryContents(directory, destDir);
        }
    }

    private async Task<Result> RunCommand(string temporaryDirectory, string pckgDirectory) =>
        await _mediator.RunNipkgRequestAsync(new string[] { "pack", temporaryDirectory, pckgDirectory });
    
}

