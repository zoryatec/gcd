using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.ControlFile;
using Gcd.Model.Nipkg.DebianFile;
using Gcd.Model.Nipkg.InstructionFile;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;
using System.Threading;

namespace Gcd.Handlers.Nipkg.Builder;

public record InitRequest(BuilderRootDir RootDir, IReadOnlyList<ControlFileProperty> ControlProperties, Maybe<LocalFilePath> BaseInstructionFilePath, Maybe<LocalFilePath> BaseControlFilePath) : IRequest<Result>;

public class InitHandler(IMediator _mediator, IFileSystem _writer)
    : IRequestHandler<InitRequest, Result>
{
    public async Task<Result> Handle(InitRequest request, CancellationToken cancellationToken)
    {
        var (rootdir, controlProperties,instrFilePath,ctrFilePath) = request;

        var defR = PackageBuilderDefinition.Of(rootdir);
        if (defR.IsFailure) return defR;

        var def = defR.Value;

        string controlFileContent = ControlFileContent.Default.Content.ToString();
        string instrFileContent = InstructionFileContent.Default.ToString();

        if (instrFilePath.HasValue)
        {
            var res = await _writer.ReadTextFileAsync(instrFilePath.Value);
            instrFileContent = res.Value;
        }

        if (ctrFilePath.HasValue)
        {
            var res = await _writer.ReadTextFileAsync(ctrFilePath.Value);
            controlFileContent = res.Value;
        }

        return await CreatePackageBuilderStructure(def)
        .Bind(() => _writer.WriteTextFileAsync(def.DebianFile, DebianFileContent.Default.Value))
        .Bind(() => _writer.WriteTextFileAsync(def.ControlFile, controlFileContent))
        .Bind(() => _writer.WriteTextFileAsync(def.InstructionFile, instrFileContent))
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
        Maybe<LocalFilePath> baseInstructionFilePath,
        Maybe<LocalFilePath> baseControlFilePath,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new InitRequest(packageContentDir, controlProperties, baseInstructionFilePath, baseControlFilePath), cancellationToken);

    public static async Task<Result> PackageBuilderSetPropertiesAsync(this IMediator mediator, BuilderRootDir packageBuilderRootDir, IReadOnlyList<ControlFileProperty> properties, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, properties), cancellationToken);
    public static async Task<Result> PackageBuilderSetPropertyAsync(this IMediator mediator, BuilderRootDir packageBuilderRootDir, ControlFileProperty property, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, new List<ControlFileProperty> { property }), cancellationToken);
}

