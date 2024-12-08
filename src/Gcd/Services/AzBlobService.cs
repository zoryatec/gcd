using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CSharpFunctionalExtensions;
using Gcd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

public class AzBlobService : IDownloadAzBlobService, IUploadAzBlobService
{

    public async Task<Result> UploadFileAsync(AzBlobUri blobUri, FilePath filePath) =>

           await UploadCoreAsync(blobUri.Value, filePath.Value);
  

    private async Task<Result> UploadCoreAsync(string fileUrl, string fileToUploadPath)
    {
        try
        {
            var blobClient = new BlobClient(new Uri(fileUrl));
            await blobClient.UploadAsync(fileToUploadPath, overwrite: true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message); 
        }

    }

    public async Task<Result> DownloadFileAsync(AzBlobUri blobUri, FilePath filePath) =>
        await Result.Try(
            async () => await DownloadCore(blobUri.Value, filePath.Value),
            ex => ex.Message);

    private async Task DownloadCore(string fileUrl, string downloadPath)
    {
        //var blobClient = new BlobClient(new Uri(fileUrl));
        //await blobClient.DownloadToAsync(fileToUploadPath);

        using (WebClient client = new WebClient())
        {
            // Download the file asynchronously
            client.DownloadFile(fileUrl, downloadPath);
            Console.WriteLine($"File downloaded successfully to {downloadPath}");
        }
 
    }
}
