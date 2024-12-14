using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadNipkg;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Model;
using Gcd.Services;
using MediatR;
using System.Threading;

namespace Gcd.Commands.NipkgPackageBuilderInit;

public record PackageBuilderInitRequest(PackageBuilderRootDir PackagePath, PackageInstalationDir PackageInstalationDir, IReadOnlyList<ControlFileProperty> ControlProperties) : IRequest<Result>;

public class PackageBuilderInitHandler(IMediator _mediator)
    : IRequestHandler<PackageBuilderInitRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderInitRequest request, CancellationToken cancellationToken)
    {
        var ControlProp = request.ControlProperties;
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath, request.PackagePath.Value);
        var pckBuilderDest = LocalDirPath.Parse(packageDirectoryPath);
        var pckDefinitionRes = PackageBuilderDefinition.Of(pckBuilderDest.Value);
        var pckDefiniton = pckDefinitionRes.Value;


        if (Directory.Exists(pckDefiniton.RootDir.Value)) Directory.Delete(packageDirectoryPath, true);
        Directory.CreateDirectory(pckDefiniton.RootDir.Value);
        Directory.CreateDirectory(pckDefiniton.DataDir.Value);
        Directory.CreateDirectory(pckDefiniton.ControlDir.Value);

        File.WriteAllText(pckDefiniton.DebianFile.Value, DebianFileContent.Default.Value);

        var controlFileContent = (ControlFileContent.Default).Content.ToString();

        File.WriteAllText(pckDefiniton.ControlFile.Value, controlFileContent);

        var instructionFileContent = new InstructionFileContent().ToString();

        File.WriteAllText(pckDefiniton.InstructionFile.Value, instructionFileContent);

        var result = await _mediator.PackageBuilderSetPropertiesAsync(request.PackagePath, ControlProp, cancellationToken);

        return Result.Success();
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderInitAsync(
        this IMediator mediator,
        PackageBuilderRootDir packageContentDir,
        PackageInstalationDir packageInstalationDir,
        IReadOnlyList<ControlFileProperty> controlProperties,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuilderInitRequest(packageContentDir, packageInstalationDir, controlProperties), cancellationToken);
}

