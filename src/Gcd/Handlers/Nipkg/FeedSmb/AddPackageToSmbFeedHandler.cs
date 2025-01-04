using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using System.IO.Compression;
using Gcd.Model.File;
using Gcd.Services.RemoteFileSystem;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;

namespace Gcd.Handlers.Nipkg.FeedSmb;


public class AddPackageToSmbFeedHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystemAzBlob _rfs)
    : IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(AddPackageToRemoteFeedRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (feedDef, packagePath, cmdPath, useAbs, createFeed) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullFeedMetaAsync(feedDef, localFeedDef.Value))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePath, cmdPath, useAbs, createFeed))
            .Bind(() => _mediator.PushFeedMetaDataAsync(feedDef, localFeedDef.Value, cancellationToken));
            //.Bind(() => UploadPackage(feedDef.Feed, insideFeedPkgPath.Value));
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));


    private async Task<Result> UploadPackage(IDirectoryDescriptor dirDescriptor, PackageFilePath packagePath)
    {

        var blorUriRes = _rfs.CreateFileDescriptor(dirDescriptor, packagePath.FileName);

        var result = await _rfs.UploadFileAsync(blorUriRes.Value, packagePath);
        return result;
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> AddPackageToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator,
        TFeedDefinition remoteFeedDef,
        IPackageFileDescriptor PackagePath,
        NipkgCmdPath cmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new AddPackageToRemoteFeedRequest<TFeedDefinition>(remoteFeedDef, PackagePath, cmdPath, useAbsolutePath, createFeed), cancellationToken);
}



