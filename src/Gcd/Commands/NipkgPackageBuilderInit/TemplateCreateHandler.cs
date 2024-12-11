using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadNipkg;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuilderInit;

public record PackageBuilderInitRequest(PackageBuilderRootDir PackagePath, PackageName PackageName, PackageVersion PackageVersion, PackageInstalationDir PackageInstalationDir) : IRequest<Result>;

public class TemplateCreateHandler()
    : IRequestHandler<PackageBuilderInitRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderInitRequest request, CancellationToken cancellationToken)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath, request.PackagePath.Value);
        var pckBuilderDest = LocalDirPath.Of(packageDirectoryPath);
        var pckDefinitionRes = PackageBuilderDefinition.Of(pckBuilderDest.Value, request.PackageInstalationDir);
        var pckDefiniton = pckDefinitionRes.Value;


        if (Directory.Exists(pckDefiniton.RootDir.Value)) Directory.Delete(packageDirectoryPath, true);
        Directory.CreateDirectory(pckDefiniton.RootDir.Value);
        Directory.CreateDirectory(pckDefiniton.DataDir.Value);
        Directory.CreateDirectory(pckDefiniton.ControlDir.Value);

        File.WriteAllText(pckDefiniton.DebianFile.Value, "2.0");

        var controlFileContent = new ControlFileContent(request.PackageName, request.PackageVersion).ToString();

        File.WriteAllText(pckDefiniton.ControlFile.Value, controlFileContent);

        var instructionFileContent = new InstructionFileContent().ToString();

        File.WriteAllText(pckDefiniton.InstructionFile.Value, instructionFileContent);

        return Result.Success();
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderInitAsync(
        this IMediator mediator,
        PackageBuilderRootDir packageContentDir,
        PackageName packageName, 
        PackageVersion packageVersion, 
        PackageInstalationDir packageInstalationDir,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuilderInitRequest(packageContentDir,  packageName, packageVersion, packageInstalationDir), cancellationToken);
}

