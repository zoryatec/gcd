using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using MediatR;
using System;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record FeedUri
{
    public static Result<FeedUri> Create(Maybe<string> feedUriOrNothing)
    {
        //Result.Try(() => new Uri(feedUriOrNothing.Value), ex => "error invalid uri format"));
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .Bind(CreateUri)
            .Map(feedUri => new FeedUri(feedUri));
    }

    private static Result<Uri> CreateUri(string uri)
    {
        try
        {
            return Result.Of<Uri>(new Uri(uri));
        }
        catch(Exception ex)
        {
            return Result.Failure<Uri>(ex.Message);
        }
    }

    private FeedUri(Uri value) => _uri = value;

    private Uri _uri;

    public string Full { get => _uri.AbsoluteUri; }
    public string BaseUri { get => _uri.GetLeftPart(UriPartial.Path); }
    public string Query { get => _uri.Query; }
}

public record FeedPath
{
    public static Result<FeedPath> Create(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("FeedUri should not be empty")
            .Ensure(value => value != string.Empty, "FeedUri should not be empty")
            .Map(value => new FeedPath(value));
    }
    private FeedPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}

public record NipkgPushAzBlobFeedMetaRequest(FeedUri FeedUri, FeedPath FeedLocalDir) : IRequest<UnitResult<Error>>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler()
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var localFeedPath = request.FeedLocalDir;

        var feedBaseUr = request.FeedUri.BaseUri;
        var queryString = request.FeedUri.Query;

        return await UploadMany(feedBaseUr, queryString, localFeedPath.ToString(),
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

