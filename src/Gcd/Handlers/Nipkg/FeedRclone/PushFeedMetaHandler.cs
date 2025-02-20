using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.RemoteFileSystem.Rclone.Abstractions;
using MediatR;


namespace Gcd.Handlers.Nipkg.FeedRclone;


public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemRclone _rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionRclone>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionRclone> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDefinition, localFeedDef) = request;
        var result = await _rfs.UploadFileAsync(remoteFeedDefinition.Package, localFeedDef.Package)
            .Bind(() => _rfs.UploadFileAsync(remoteFeedDefinition.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.UploadFileAsync(remoteFeedDefinition.PackageStamps, localFeedDef.PackageStamps));
        return result.MapError(er => er.Message);
    }
}
