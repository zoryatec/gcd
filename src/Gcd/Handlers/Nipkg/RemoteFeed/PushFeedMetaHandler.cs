using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;


namespace Gcd.Handlers.Nipkg.RemoteFeed;

public record NipkgPushAzBlobFeedMetaRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDefinition) : IRequest<Result>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _rfs.UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}

public static class MediatorExtensionsPush
{
    public static async Task<Result> PushFeedMetaDataAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPushAzBlobFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
}