﻿using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.FeedDefinition;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;


namespace Gcd.Handlers.Nipkg.FeedAzBlob;


public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _rfs.UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}