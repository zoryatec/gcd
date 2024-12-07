using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using Gcd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

public class AzBlobService : IDownloadAzBlobService, IUploadAzBlobService
{
    public Task<UnitResult<Error>> DownloadFileAsync(AzBlobUri blobUri, FilePath filePath)
    {
        throw new NotImplementedException();
    }

    public async Task<UnitResult<Error>> UploadFileAsync(AzBlobUri blobUri, FilePath filePath)=>
    await Result.Try<Error>(
        async () => await UploadCore(blobUri.Value, filePath.Value),
        ex => new Error(ex.Message));

    private async Task UploadCore(string fileUrl, string fileToUploadPath)
    {
        var blobClient = new BlobClient(new Uri(fileUrl));
        await blobClient.UploadAsync(fileToUploadPath, overwrite: true);
    }
}
