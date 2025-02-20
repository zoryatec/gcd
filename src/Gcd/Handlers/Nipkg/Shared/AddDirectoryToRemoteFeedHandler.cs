using CSharpFunctionalExtensions;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.RemoteFileSystem;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Handlers.Nipkg.Shared;

public record AddDirectoryToRemoteFeedRequest<TFeedDefinition>(
    TFeedDefinition FeedDefinition,
    ILocalDirPath PackageSourceDirectory,
    NipkgCmdPath CmdPath,
    UseAbsolutePath useAbsolutePath,
    bool createFeed = false
    )
    : IRequest<Result> where TFeedDefinition : IFeedDefinition;

public class AddDirectoryToRemoteFeedHandler<TFeedDefinition>(
    IMediator _mediator,
    IFileSystem _fs)
    : IRequestHandler<AddDirectoryToRemoteFeedRequest<TFeedDefinition>,Result> where TFeedDefinition : IFeedDefinition

{
    public async Task<Result> Handle(AddDirectoryToRemoteFeedRequest<TFeedDefinition> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageDir, cmdPath, useAbs, createFeed) = request;
        
        var paths = _fs.ListFiles(packageDir,"*.nipkg", recursive: true);

        if (!paths.Value.Any()) return Result.Failure($"No packages found in {packageDir.Value}");
        List<IPackageFileDescriptor> packageFiles = new List<IPackageFileDescriptor>();
        foreach (var path in paths.Value)
        {
            var path1 = PackageLocalFilePath.Of(path.Value);
            if(path1.IsFailure) return path1.MapError(er => er.Message);;
            packageFiles.Add(path1.Value);
        }
        
        var result = await _mediator.AddPackageToRemoteFeedAsync(feedDef, packageFiles, cmdPath,useAbs, createFeed,cancellationToken);
        return result;
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

}

