﻿

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
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Common;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgAddPackageToAzFeed;

public record PackagePath
{
    public string Value { get; }

    public static Result<PackagePath> Create(string packagePath)
    {
        return Result.Success(new PackagePath(packagePath));
    }

    private PackagePath(string path)
    {
        Value = path;
    }
}

 public record AddPackageToFeedRequest(FeedUri FeedUri, PackagePath PackagePath) : IRequest<UnitResult<Error>>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(IMediator mediator)
    : IRequestHandler<AddPackageToFeedRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string currentDirectoryPath = Environment.CurrentDirectory;
        //string tempPckDirName = "tempFeed";
        //string temporaryDirectory = Path.Combine(currentDirectoryPath, tempPckDirName);

        var localFeedPath = temporaryDirectory;
        var downloadReq = new NipkgPullFeedMetaRequest(request.FeedUri.Full, localFeedPath);
        string packageName = Path.GetFileName(request.PackagePath.Value);
        var packageDestinationPath = Path.Combine(localFeedPath, packageName);
        CancellationTokenSource cts = new CancellationTokenSource();
        var downloadResult = await mediator.Send(downloadReq);

        File.Copy(request.PackagePath.Value, packageDestinationPath, true);
        Console.WriteLine("Package copied to temp feed:");

        AddPackageToLcalFeed(localFeedPath, packageDestinationPath);
        Console.WriteLine("Package added to temp feed:");

        string feedBaseUr = request.FeedUri.BaseUri;
        string queryString = request.FeedUri.Query;

        var pushRequest = new NipkgPushAzBlobFeedMetaRequest(request.FeedUri, FeedPath.Create(localFeedPath).Value);
        var pushResult = await mediator.Send(pushRequest);

        if (pushResult.IsFailure) return pushResult;

        string nipkgUrl = CreateSubUrl(feedBaseUr, packageName, queryString);
        await Upload(nipkgUrl, $"{localFeedPath}\\{packageName}");

        Directory.Delete(temporaryDirectory, true);

        return UnitResult.Success<Error>();
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }

    private void AddPackageToLcalFeed(string feedDir, string packagePath)
    {
        // Initialize the ProcessStartInfo with the command
        string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";
        string arguments = $"/c {nipkg} feed-add-pkg {feedDir} {packagePath}";


        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",       // Use "cmd.exe" to run a command
            Arguments = arguments, // "/c" tells cmd to run the command and then terminate
            RedirectStandardOutput = true, // Redirect the output of the command
            RedirectStandardError = true,  // Redirect any errors
            UseShellExecute = false,      // Don't use the shell to execute the command
            CreateNoWindow = true        // Don't create a command window
        };

        try
        {
            using (Process process = Process.Start(startInfo))
            {
                // Read the standard output and error
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                // Wait for the command to finish
                process.WaitForExit();

                // Print the output and errors (if any)
                Console.WriteLine("Output:");
                Console.WriteLine(output);

                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running command: {ex.Message}");
        }
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


