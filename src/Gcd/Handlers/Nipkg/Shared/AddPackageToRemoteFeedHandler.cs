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
    IPackageFileDescriptor PackagePath,
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
        var (azFeedDef, packagePath, cmdPath, useAbs, createFeed) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullFeedMetaAsync(azFeedDef, localFeedDef.Value))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePath, cmdPath, useAbs, createFeed))
            .Bind(() => _mediator.PushFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => _mediator.UploadPackageAsync(azFeedDef, insideFeedPkgPath.Value));
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

}

