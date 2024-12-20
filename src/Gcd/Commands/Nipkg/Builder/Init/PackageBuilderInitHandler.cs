using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Model;
using Gcd.Services.FileSystem;
using MediatR;
using System.Threading;

namespace Gcd.Commands.Nipkg.Builder.Init;

public record PackageBuilderInitRequest(PackageBuilderRootDir RootDir, InatallationTargetRootDir InstalationDir, IReadOnlyList<ControlFileProperty> ControlProperties) : IRequest<Result>;

public class PackageBuilderInitHandler(IMediator _mediator, IFileSystem _writer)
    : IRequestHandler<PackageBuilderInitRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderInitRequest request, CancellationToken cancellationToken)
    {
        var (rootdir, installationDir, controlProperties) = request;

        var defR = PackageBuilderDefinition.Of(rootdir);
        if (defR.IsFailure) return defR;

        var def = defR.Value;

        return await CreatePackageBuilderStructure(def)
        .Bind(() => _writer.WriteTextFileAsync(def.DebianFile, DebianFileContent.Default.Value))
        .Bind(() => _writer.WriteTextFileAsync(def.ControlFile, ControlFileContent.Default.Content.ToString()))
        .Bind(() => _writer.WriteTextFileAsync(def.InstructionFile, InstructionFileContent.Default.ToString()))
        .Bind(() => _mediator.PackageBuilderSetPropertiesAsync(request.RootDir, controlProperties, cancellationToken));
    }

    private Result CreatePackageBuilderStructure(PackageBuilderDefinition def)
    {
        try
        {
            if (Directory.Exists(def.RootDir.Value)) Directory.Delete(def.RootDir.Value, true);
            Directory.CreateDirectory(def.RootDir.Value);
            Directory.CreateDirectory(def.DataDir.Value);
            Directory.CreateDirectory(def.ControlDir.Value);
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
    public static async Task<Result> PackageBuilderInitAsync(
        this IMediator mediator,
        PackageBuilderRootDir packageContentDir,
        InatallationTargetRootDir packageInstalationDir,
        IReadOnlyList<ControlFileProperty> controlProperties,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuilderInitRequest(packageContentDir, packageInstalationDir, controlProperties), cancellationToken);
}

