
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

public record NipkgPullFeedMetaRequest(FeedUri FeedUri, FeedPath FeedLocalDir) : IRequest<UnitResult<Error>>;

public class NipkgPullFeedMetaHandler(IDownloadAzBlobService downloadService)
    : IRequestHandler<NipkgPullFeedMetaRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        string feedBaseUr = request.FeedUri.BaseUri;
        string queryString = request.FeedUri.Query;



        if (!Directory.Exists(request.FeedLocalDir.Value))
        {
            // If it doesn't exist, create it
            Directory.CreateDirectory(request.FeedLocalDir.Value);
        }

        //string packageUrl = CreateSubUrl(feedBaseUr, "Packages", queryString);
        //DownloadFile(packageUrl, Path.Combine(request.FeedLocalDir.Value, "Packages"));
        //string packageGzUrl = CreateSubUrl(feedBaseUr, "Packages.gz", queryString);
        //DownloadFile(packageGzUrl, Path.Combine(request.FeedLocalDir.Value, "Packages.gz"));
        //string packageStampsUrl = CreateSubUrl(feedBaseUr, "Packages.stamps", queryString);
        //DownloadFile(packageStampsUrl, Path.Combine(request.FeedLocalDir.Value, "Packages.stamps"));

        return await DownloadMany(request.FeedUri, request.FeedLocalDir,
            "Packages",
            "Packages.gz",
            "Packages.stamps");



        //return new NipkgPullFeedMetaRespons("");
    }


    private async Task<UnitResult<Error>> DownloadMany(FeedUri feedUri, FeedPath feedLocalDir, params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            var fileUri = CreateSubUrl(feedUri.BaseUri, fileName, feedUri.Query);
            var blobUri = AzBlobUri.Create(fileUri);
            var filePath = FilePath.Create($"{feedLocalDir}\\{fileName}");

            var result = await downloadService.DownloadFileAsync(blobUri.Value, filePath.Value);
            if (result.IsFailure) return result;
        }
        return UnitResult.Success<Error>();
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }

    private void DownloadFile(string fileUrl, string downloadPath)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                // Download the file asynchronously
                client.DownloadFile(fileUrl, downloadPath);
                Console.WriteLine($"File downloaded successfully to {downloadPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
        }
    }
}

