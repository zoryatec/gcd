using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Model;
using Gcd.Services;
using Gcd.Tests.EndToEnd;
using MediatR;
using System.Reflection.Metadata;

namespace Gcd.Commands.NipkgPackageBuild;

public record PackageBuildRequest(PackageBuilderContentSourceDir PackageContentPath, PackageInstalationDir PackageInstalationDir, PackageDestinationDirectory PackageDestinationDir, IReadOnlyList<ControlFileProperty> ControlProperties) : IRequest<Result>;


public class PackageBuildHandler(IMediator _mediator, ITempDirectoryProvider _tempDir, IFileSystem _fs)
    : IRequestHandler<PackageBuildRequest, Result>
{
    public async Task<Result> Handle(PackageBuildRequest request, CancellationToken cancellationToken)
    {
        var (contentSrcDir, installationDir, outputDir, controlProp) = request;

        var rootDirTempR = await _tempDir.GenerateTempDirectoryAsync()
            .Bind(dir => PackageBuilderRootDir.Of(dir.Value));
 
        var rootDirTemp = rootDirTempR.Value;
        var contentDirResult = PackageBuilderContentDir.Of(rootDirTemp, installationDir);
        var contentDstDir = contentDirResult.Value;


        // build package
        return  await _mediator
            .PackageBuilderInitAsync(rootDirTemp, installationDir, controlProp)
            .Bind(() => CopyPackageContentAsync(contentSrcDir, contentDstDir))
            .Bind(() => NipkgPackAsync(rootDirTemp, outputDir));

 
    }

    static async Task<Result> CopyPackageContentAsync(PackageBuilderContentSourceDir source, PackageBuilderContentDir dest)
    {
       return Result.Try(() => CopyDirectoryContents(source.Value, dest.Value));
        
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

    private async Task<Result> NipkgPackAsync(PackageBuilderRootDir rootDir, PackageDestinationDirectory destDirectory) =>
        await _mediator.RunNipkgRequestAsync(new string[] { "pack", rootDir.Value, destDirectory.Value });
    
}

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderBuildAsync(
        this IMediator mediator,
        PackageBuilderContentSourceDir packageContentDir,
        PackageInstalationDir packageInstalationDir,
        PackageDestinationDirectory packageDestinationDir,
        IReadOnlyList<ControlFileProperty> controlProperties,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuildRequest(packageContentDir, packageInstalationDir, packageDestinationDir, controlProperties), cancellationToken);
}