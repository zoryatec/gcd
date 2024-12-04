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

public record NipkgPushAzBlobFeedMetaRequest(string FeedUri, string FeedLocalDir) : IRequest<NipkgPushAzBlobFeedMetaRespons>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler()
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, NipkgPushAzBlobFeedMetaRespons>
{
    public async Task<NipkgPushAzBlobFeedMetaRespons> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var localFeedPath = request.FeedLocalDir;
        Uri uri = new Uri(request.FeedUri);
        string feedBaseUr = uri.GetLeftPart(UriPartial.Path);
        string queryString = uri.Query;


        string packageUrl = CreateSubUrl(feedBaseUr, "Packages", queryString);
        await Upload(packageUrl, $"{localFeedPath}\\Packages");
        string packageGzUrl = CreateSubUrl(feedBaseUr, "Packages.gz", queryString);
        await Upload(packageGzUrl, $"{localFeedPath}\\Packages.gz");
        string packageStampsUrl = CreateSubUrl(feedBaseUr, "Packages.stamps", queryString);
        await Upload(packageStampsUrl, $"{localFeedPath}\\Packages.stamps");


        return new NipkgPushAzBlobFeedMetaRespons("");
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }

    private async Task Upload(string fileUrl, string fileToUploadPath)
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

            Console.WriteLine("Blob uploaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

