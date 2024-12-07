using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using Gcd.Common;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record NipkgPushAzBlobFeedMetaRequest(string FeedUri, string FeedLocalDir) : IRequest<UnitResult<Error>>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler()
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var localFeedPath = request.FeedLocalDir;
        Uri uri = new Uri(request.FeedUri);
        var feedBaseUr = uri.GetLeftPart(UriPartial.Path);
        var queryString = uri.Query;


        //var packageUrl = CreateSubUrl(feedBaseUr, "Packages", queryString);
        //var result = await Upload(packageUrl, $"{localFeedPath}\\Packages");
        //var packageGzUrl = CreateSubUrl(feedBaseUr, "Packages.gz", queryString);
        //result = await Upload(packageGzUrl, $"{localFeedPath}\\Packages.gz");
        //var packageStampsUrl = CreateSubUrl(feedBaseUr, "Packages.stamps", queryString);
        //result = await Upload(packageStampsUrl, $"{localFeedPath}\\Packages.stamps");

        //if (result.IsFailure) return Result.Failure<NipkgPushAzBlobFeedMetaRespons>("ERRORR !!!!");
        //if(result.IsFailure) return result.Ensure(

        return await UploadMany(feedBaseUr, queryString, localFeedPath,
            "Packages",
            "Packages.gz",
            "Packages.stamps");
    }



    private async Task<UnitResult<Error>> UploadMany(string feedUri,string queryString, string localFeedPath, params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            var packageUrl = CreateSubUrl(feedUri, fileName, queryString);
            var result = await Upload(packageUrl, $"{localFeedPath}\\{fileName}");
            if (result.IsFailure) return result;
        }
        return UnitResult.Success<Error>();
    }

    private string CreateSubUrl(string baseUrl, string subPath, string queryParam)
    {
        return $"{baseUrl}/{subPath}{queryParam}";
    }

    private async Task<UnitResult<Error>> Upload(string fileUri, string fileToUploadPath) =>
        await Result.Try<Error>(
            async () => await UploadCore(fileUri,fileToUploadPath),
            ex => new Error(ex.Message));
    private async Task UploadCore(string fileUrl, string fileToUploadPath)
    {
        var blobClient = new BlobClient(new Uri(fileUrl));
        await blobClient.UploadAsync(fileToUploadPath, overwrite: true);
    }
}

