using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.Nipkg.Build;

public record PackageBuildRequest(
    PackageBuilderContentSourceDir PackageContentPath,
    InatallationTargetRootDir PackageInstalationDir,
    PackageDestinationDirectory PackageDestinationDir,
    IReadOnlyList<ControlFileProperty> ControlProperties,
    NipkgCmdPath CmdPath,
    Maybe<LocalFilePath> BaseInstrumentFile,
    Maybe<LocalFilePath> BaseControlFile
    ) : IRequest<Result>;


public class PackageBuildHandler(IMediator _mediator, IFileSystem _fs)
    : IRequestHandler<PackageBuildRequest, Result>
{
    public async Task<Result> Handle(PackageBuildRequest request, CancellationToken cancellationToken)
    {
        var (contentSrcDir, installationDir, outputDir, controlProp, cmd, instructionFilePath, controlFilePath) = request;

        var rootDirTempR = await _fs.GenerateTempDirectoryAsync()
            .Bind(dir => BuilderRootDir.Of(dir.Value));

        var rootDirTemp = rootDirTempR.Value;
        var contentDirResult = PackageBuilderContentDir.Of(rootDirTemp, installationDir);
        var contentDstDir = contentDirResult.Value;

        // build package
        return await _mediator
            .PackageBuilderInitAsync(rootDirTemp, controlProp, instructionFilePath, controlFilePath)
            .Bind(() => _mediator.AddContentAsync(rootDirTemp, installationDir, contentSrcDir))
            .Bind(() => _mediator.BuilderPackAsync(rootDirTemp, outputDir, cmd));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderBuildAsync(
        this IMediator mediator,
        PackageBuilderContentSourceDir packageContentDir,
        InatallationTargetRootDir packageInstalationDir,
        PackageDestinationDirectory packageDestinationDir,
        IReadOnlyList<ControlFileProperty> controlProperties,
        NipkgCmdPath cmdPath,
        Maybe<LocalFilePath> baseInstrumentFile,
        Maybe<LocalFilePath> baseControlFile,
        CancellationToken cancellationToken = default
        )
        => await mediator.Send(new PackageBuildRequest(packageContentDir, packageInstalationDir, packageDestinationDir, controlProperties, cmdPath, baseInstrumentFile, baseControlFile), cancellationToken);
}