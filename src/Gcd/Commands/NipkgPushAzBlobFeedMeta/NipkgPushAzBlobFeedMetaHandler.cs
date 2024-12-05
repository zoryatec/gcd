using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record NipkgPushAzBlobFeedMetaRequest(string FeedUri, string FeedLocalDir) : IRequest<Result<NipkgPushAzBlobFeedMetaRespons>>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler()
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, Result<NipkgPushAzBlobFeedMetaRespons>>
{
    public async Task<Result<NipkgPushAzBlobFeedMetaRespons>> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var localFeedPath = request.FeedLocalDir;
        Uri uri = new Uri(request.FeedUri);
        string feedBaseUr = uri.GetLeftPart(UriPartial.Path);
        string queryString = uri.Query;

        
        string packageUrl = CreateSubUrl(feedBaseUr, "Packages", queryString);
        var result = await Upload(packageUrl, $"{localFeedPath}\\Packages");
        string packageGzUrl = CreateSubUrl(feedBaseUr, "Packages.gz", queryString);
        result = await Upload(packageGzUrl, $"{localFeedPath}\\Packages.gz");
        string packageStampsUrl = CreateSubUrl(feedBaseUr, "Packages.stamps", queryString);
        result = await Upload(packageStampsUrl, $"{localFeedPath}\\Packages.stamps");

        if (result.IsFailure) return Result.Failure<NipkgPushAzBlobFeedMetaRespons>("ERRORR !!!!");

        return Result.Success<NipkgPushAzBlobFeedMetaRespons>(new NipkgPushAzBlobFeedMetaRespons(""));
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }

    private async Task<Result> Upload(string fileUrl, string fileToUploadPath)
    {
        // Blob URL with SAS token (including the full query string)
        string blobUrlWithSas = fileUrl;

        // Path to the file to upload
        string filePath = fileToUploadPath;

        // Create a BlobClient using the SAS URL
        BlobClient blobClient = new BlobClient(new Uri(blobUrlWithSas));

        try
        {
            Console.WriteLine("Uploading blob...");

            // Upload the file
            await blobClient.UploadAsync(filePath, overwrite: true);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}");
        }
    }
}

