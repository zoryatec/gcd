using CSharpFunctionalExtensions;
using Gcd.Model.Nipkg.ControlFile;
using Gcd.Model.Nipkg.DebianFile;
using Gcd.Model.Nipkg.InstructionFile;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.Services.FileSystem;
using MediatR;
using System.Threading;

namespace Gcd.Handlers.Nipkg.Builder;

public record InitRequest(BuilderRootDir RootDir, IReadOnlyList<ControlFileProperty> ControlProperties) : IRequest<Result>;

public class InitHandler(IMediator _mediator, IFileSystem _writer)
    : IRequestHandler<InitRequest, Result>
{
    public async Task<Result> Handle(InitRequest request, CancellationToken cancellationToken)
    {
        var (rootdir, controlProperties) = request;

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

public static class MediatorExtensionsInit
{
    public static async Task<Result> PackageBuilderInitAsync(
        this IMediator mediator,
        BuilderRootDir packageContentDir,
        IReadOnlyList<ControlFileProperty> controlProperties,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new InitRequest(packageContentDir, controlProperties), cancellationToken);

    public static async Task<Result> PackageBuilderSetPropertiesAsync(this IMediator mediator, BuilderRootDir packageBuilderRootDir, IReadOnlyList<ControlFileProperty> properties, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, properties), cancellationToken);
    public static async Task<Result> PackageBuilderSetPropertyAsync(this IMediator mediator, BuilderRootDir packageBuilderRootDir, ControlFileProperty property, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, new List<ControlFileProperty> { property }), cancellationToken);
}

