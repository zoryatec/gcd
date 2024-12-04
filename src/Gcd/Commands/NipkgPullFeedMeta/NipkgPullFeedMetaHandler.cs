
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record DownloadFeedMetadataRequest(string FeedUrl, string FeedLocalDir) : IRequest<DownloadFeedMetadataResponse>;
public record DownloadFeedMetadataResponse(string Result);

public class NipkgPullFeedMetaHandler()
    : IRequestHandler<DownloadFeedMetadataRequest, DownloadFeedMetadataResponse>
{
    public async Task<DownloadFeedMetadataResponse> Handle(DownloadFeedMetadataRequest request, CancellationToken cancellationToken)
    {
        Uri uri = new Uri(request.FeedUrl);
        string feedBaseUr = uri.GetLeftPart(UriPartial.Path);
        string queryString = uri.Query;



        if (!Directory.Exists(request.FeedLocalDir))
        {
            // If it doesn't exist, create it
            Directory.CreateDirectory(request.FeedLocalDir);
        }

        string packageUrl = CreateSubUrl(feedBaseUr, "Packages", queryString);
        DownloadFile(packageUrl, Path.Combine(request.FeedLocalDir, "Packages"));
        string packageGzUrl = CreateSubUrl(feedBaseUr, "Packages.gz", queryString);
        DownloadFile(packageGzUrl, Path.Combine(request.FeedLocalDir, "Packages.gz"));
        string packageStampsUrl = CreateSubUrl(feedBaseUr, "Packages.stamps", queryString);
        DownloadFile(packageStampsUrl, Path.Combine(request.FeedLocalDir, "Packages.stamps"));




        return new DownloadFeedMetadataResponse("");
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

