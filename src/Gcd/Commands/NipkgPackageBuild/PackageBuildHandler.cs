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
        var (contentSrcDir, installationDir, outputDir, controlProperties) = request;

        var temporaryDirectoryR =  await _tempDir.GenerateTempDirectoryAsync();
        var tempDir = temporaryDirectoryR.Value;


        var pckgDirectoryR = await _tempDir.CreateTempDirPathAsync();
        string pckgDirectory = pckgDirectoryR.Value.Value.ToString();

        if (! (await _fs.CheckDirectoryExists(pckgDirectoryR.Value)).Value) await _fs.CreateDirectoryAsync(pckgDirectoryR.Value);


        var pckDefinitionRes = PackageBuilderDefinition.Of(tempDir);
        var pckDefiniton = pckDefinitionRes.Value;

        var rootDirTempR = PackageBuilderRootDir.Create(tempDir.Value);
        var rootDirTemp = rootDirTempR.Value;

        var subRequest = await _mediator.PackageBuilderInitAsync(rootDirTemp, request.PackageInstalationDir, request.ControlProperties);

        var contentDirResult = PackageBuilderContentDir.Of(rootDirTemp, request.PackageInstalationDir);
        var contentDstDir = contentDirResult.Value;

        await CopyPackageContentAsync(contentSrcDir, contentDstDir);

        var contentR = await _fs.ReadTextFileAsync(pckDefiniton.ControlFile);
        string content = contentR.Value;

        var cfcR = ControlFileContent.Of(content);
        var cfc = cfcR.Value;


        var result = await NipkgPackAsync(tempDir.Value, pckgDirectory);

        var packageFileNameRR = new PackageFileName(cfc.Architecture, cfc.Name, cfc.Version);
        //string packageFileName = (new PackageFileName(cfc.Architecture,cfc.Name,cfc.Version)).Value;
        var packageSrcFilePathR = PackageFilePath.Of(pckgDirectoryR.Value, packageFileNameRR);

        string packageFilePath = /*Path.Combine(pckgDirectory, packageFileName);*/ packageSrcFilePathR.Value;

        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDestinationDir = Path.Combine(currentDirectoryPath, request.PackageDestinationDir.Value);

        if ((await _fs.CheckDirectoryExists(outputDir)).Value) await _fs.CreateDirectoryAsync(outputDir);

        var packageDestFilePathR = PackageFilePath.Of(outputDir, packageFileNameRR);

        await _fs.CopyFileAsync(packageSrcFilePathR, packageDestFilePathR);

        Directory.Delete(tempDir.Value, true);
        Directory.Delete(pckgDirectory, true);

        return Result.Success();
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

    private async Task<Result> NipkgPackAsync(string temporaryDirectory, string pckgDirectory) =>
        await _mediator.RunNipkgRequestAsync(new string[] { "pack", temporaryDirectory, pckgDirectory });
    
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