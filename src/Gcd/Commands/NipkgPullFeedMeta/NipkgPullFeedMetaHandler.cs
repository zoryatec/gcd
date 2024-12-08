
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.Common;
using Gcd.LabViewProject;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record NipkgPullFeedMetaRequest(FeedUri FeedUri, FeedPath FeedLocalDir) : IRequest<Result>;

public class NipkgPullFeedMetaHandler(IDownloadAzBlobService downloadService)
    : IRequestHandler<NipkgPullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {

        if (!Directory.Exists(request.FeedLocalDir.Value))
        {
            // If it doesn't exist, create it
            Directory.CreateDirectory(request.FeedLocalDir.Value);
        }

        return await DownloadMany(request.FeedUri, request.FeedLocalDir,
            "Packages",
            "Packages.gz",
            "Packages.stamps");
    }

    private async Task<Result> DownloadMany(FeedUri feedUri, FeedPath feedLocalDir, params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            var fileUri = CreateSubUrl(feedUri.BaseUri, fileName, feedUri.Query);
            var blobUri = AzBlobUri.Create(fileUri);
            var filePath = FilePath.Create($"{feedLocalDir}\\{fileName}");

            var result = await downloadService.DownloadFileAsync(blobUri.Value, filePath.Value);
            if (result.IsFailure) return result;
        }
        return Result.Success();
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }
}

