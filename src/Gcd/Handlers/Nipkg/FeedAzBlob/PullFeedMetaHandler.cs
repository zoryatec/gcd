﻿using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.FeedDefinition;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedAzBlob;

public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}