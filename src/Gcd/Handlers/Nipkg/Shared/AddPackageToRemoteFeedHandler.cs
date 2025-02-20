using CSharpFunctionalExtensions;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.RemoteFileSystem;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Handlers.Nipkg.Shared;

public record AddPackageToRemoteFeedRequest<TFeedDefinition>(
    TFeedDefinition AzFeedDef,
    IReadOnlyList<IPackageFileDescriptor> PackagePaths,
    NipkgCmdPath CmdPath,
    UseAbsolutePath useAbsolutePath,
    bool createFeed = false
    )
    : IRequest<Result> where TFeedDefinition : IFeedDefinition;

public class AddPackageToRemoteFeedHandler<TFeedDefinition>(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystemAzBlob _rfs)
    : IRequestHandler<AddPackageToRemoteFeedRequest<TFeedDefinition>,Result> where TFeedDefinition : IFeedDefinition

{
    public async Task<Result> Handle(AddPackageToRemoteFeedRequest<TFeedDefinition> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePaths, cmdPath, useAbs, createFeed) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPaths = new List<PackageLocalFilePath>();
        foreach (var packagePath in packagePaths)
        {
            var insideFeedPkgPath = localFeedDef
                .Map((arg) => PackageLocalFilePath.Of(arg.Feed, packagePath.FileName));
            if (insideFeedPkgPath.IsFailure) return insideFeedPkgPath;
            insideFeedPkgPaths.Add(insideFeedPkgPath.Value);
        }
        
        return await Result.Combine(localFeedDef)
            .Bind(() => _mediator.PullFeedMetaAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePaths, cmdPath, useAbs, createFeed,cancellationToken))
            .Bind(() => _mediator.PushFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => _mediator.UploadPackageAsync(azFeedDef, insideFeedPkgPaths,cancellationToken));
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

}

