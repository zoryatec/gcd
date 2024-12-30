using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Commands.Nipkg.Feed.PullMetaDataAz;
using Gcd.Commands.Nipkg.Feed.PushMetaDataAz;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using System.IO.Compression;
using Gcd.Model.File;
using Gcd.Commands.Nipkg.FeedLocal.AddPackageLocal;
using Gcd.Model.FeedDefinition;
using Gcd.Services.RemoteFileSystem;

namespace Gcd.Commands.Nipkg.Feed.AddPackageAz;

public record AddPackageToAzFeedRequest(IFeedDefinition AzFeedDef, IPackageFileDescriptor PackagePath, NipkgCmdPath CmdPath) : IRequest<Result>;
public record AddPackageToAzFeedResponse(string Result);

public class AddPackageToAzFeedHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystem _rfs)
    : IRequestHandler<AddPackageToAzFeedRequest, Result>
{
    public async Task<Result> Handle(AddPackageToAzFeedRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePath, cmdPath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullFeedMetaAsync(azFeedDef, localFeedDef.Value))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value,packagePath, cmdPath, UseAbsolutePath.No))
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



