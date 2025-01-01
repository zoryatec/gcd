using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using System.IO.Compression;
using Gcd.Model.File;
using Gcd.Model.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using Gcd.Handlers.Nipkg.FeedLocal;

namespace Gcd.Handlers.Nipkg.RemoteFeed;

public record AddPackageToRemoteFeedRequest<TFeedDefinition>(TFeedDefinition AzFeedDef, IPackageFileDescriptor PackagePath, NipkgCmdPath CmdPath) 
        : IRequest<Result> where TFeedDefinition : IFeedDefinition;
public record AddPackageToRemoteFeedResponse(string Result);

public class AddPackageToRemoteFeedHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystem _rfs)
    : IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(AddPackageToRemoteFeedRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePath, cmdPath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullFeedMetaAsync(azFeedDef, localFeedDef.Value))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePath, cmdPath, UseAbsolutePath.No))
            .Bind(() => _mediator.PushFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => UploadPackage(azFeedDef.Feed, insideFeedPkgPath.Value));
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
    public static async Task<Result> AddPackageToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, IPackageFileDescriptor PackagePath, NipkgCmdPath cmdPath, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new AddPackageToRemoteFeedRequest<TFeedDefinition>(remoteFeedDef, PackagePath, cmdPath), cancellationToken);
}



