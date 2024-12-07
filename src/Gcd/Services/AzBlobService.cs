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

    public async Task<UnitResult<Error>> UploadFileAsync(AzBlobUri blobUri, FilePath filePath)=>
        await Result.Try<Error>(
            async () => await UploadCore(blobUri.Value, filePath.Value),
            ex => new Error(ex.Message));

    private async Task UploadCore(string fileUrl, string fileToUploadPath)
    {
        var blobClient = new BlobClient(new Uri(fileUrl));
        await blobClient.UploadAsync(fileToUploadPath, overwrite: true);
    }

    public async Task<UnitResult<Error>> DownloadFileAsync(AzBlobUri blobUri, FilePath filePath) =>
        await Result.Try<Error>(
            async () => await DownloadCore(blobUri.Value, filePath.Value),
            ex => new Error(ex.Message));

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
