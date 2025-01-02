using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.FeedDefinition;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;


namespace Gcd.Handlers.Nipkg.FeedSmb;

public class PushFeedMetaHandler(IFileSystem _fs, RemoteFileSystemSmb _rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _rfs.UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}