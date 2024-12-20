using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Commands.Nipkg.Builder.Pack;
using Gcd.Model;
using Gcd.Model.Config;
using Gcd.Services;
using Gcd.Tests.EndToEnd;
using MediatR;

namespace Gcd.Commands.Nipkg.Build;

public record PackageBuildRequest(
    PackageBuilderContentSourceDir PackageContentPath,
    InatallationTargetRootDir PackageInstalationDir,
    PackageDestinationDirectory PackageDestinationDir, 
    IReadOnlyList<ControlFileProperty> ControlProperties,
    NipkgCmdPath CmdPath) : IRequest<Result>;


public class PackageBuildHandler(IMediator _mediator, ITempDirectoryProvider _tempDir, IFileSystem _fs)
    : IRequestHandler<PackageBuildRequest, Result>
{
    public async Task<Result> Handle(PackageBuildRequest request, CancellationToken cancellationToken)
    {
        var (contentSrcDir, installationDir, outputDir, controlProp,cmd) = request;

        var rootDirTempR = await _tempDir.GenerateTempDirectoryAsync()
            .Bind(dir => PackageBuilderRootDir.Of(dir.Value));

        var rootDirTemp = rootDirTempR.Value;
        var contentDirResult = PackageBuilderContentDir.Of(rootDirTemp, installationDir);
        var contentDstDir = contentDirResult.Value;


        // build package
        return await _mediator
            .PackageBuilderInitAsync(rootDirTemp, installationDir, controlProp)
            .Bind(() => _mediator.AddContentAsync(rootDirTemp, installationDir, contentSrcDir))
            .Bind(() => _mediator.BuilderPackAsync(rootDirTemp, outputDir,cmd));
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
        CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuildRequest(packageContentDir, packageInstalationDir, packageDestinationDir, controlProperties, cmdPath), cancellationToken);
}