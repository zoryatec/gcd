using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using System.Net;

namespace Gcd.Services;


public record AzBlobUri : IFileDescriptor
{
    public static Result<AzBlobUri> Create(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult("FeedUri should not be empty")
        .Ensure(blobUri => blobUri != string.Empty, "FeedUri should not be empty")
        .MapTry((blobUri) => new Uri(blobUri), ex => ex.Message)
        .Map(blobUri => new AzBlobUri(blobUri));
    }
    private AzBlobUri(Uri value) => _uri = value;
    private Uri _uri;
    public string Value { get => _uri.AbsoluteUri; }
}
public interface IUploadAzBlobService
{
    public Task<Result> UploadFileAsync(AzBlobUri blobUri, ILocalFilePath filePath);
}


public interface IDownloadAzBlobService
{
    public Task<Result> DownloadFileAsync(AzBlobUri blobUri, ILocalFilePath filePath);
}

public class AzBlobService : IDownloadAzBlobService, IUploadAzBlobService
{

    public async Task<Result> UploadFileAsync(AzBlobUri blobUri, ILocalFilePath filePath) =>

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

    public async Task<Result> DownloadFileAsync(AzBlobUri blobUri, ILocalFilePath filePath) =>
        await Result.Try(
            async () => await DownloadCore(blobUri.Value, filePath.Value),
            ex => ex.Message);

    private async Task DownloadCore(string fileUrl, string downloadPath)
    {

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(fileUrl, downloadPath);
            Console.WriteLine($"File downloaded successfully to {downloadPath}");
        }
 
    }
}
