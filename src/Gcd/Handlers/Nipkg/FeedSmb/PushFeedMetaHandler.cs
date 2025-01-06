using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;
using Gcd.Common;


namespace Gcd.Handlers.Nipkg.FeedSmb;

public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemSmb _rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;
        return await _rfs.UploadFileAsync(remoteFeedDef.Feed, remoteFeedDef.Package, localFeedDef.Package, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword)
            .Bind(() => _rfs.UploadFileAsync(remoteFeedDef.Feed, remoteFeedDef.PackageGz, localFeedDef.PackageGz, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword))
            .Bind(() => _rfs.UploadFileAsync(remoteFeedDef.Feed, remoteFeedDef.PackageStamps, localFeedDef.PackageStamps, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword))
            .MapError(err => err.Message);
    }
}