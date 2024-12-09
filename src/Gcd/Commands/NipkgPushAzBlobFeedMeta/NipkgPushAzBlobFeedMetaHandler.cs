using Azure.Core;
using Azure.Storage.Blobs;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Common;
using Gcd.Services;
using MediatR;
using System;

namespace Gcd.Commands.NipkgPushAzBlobFeedMeta;

public record FeedUri
{
    public static Result<FeedUri> Create(Maybe<string> feedUriOrNothing)
    {
        //Result.Try(() => new Uri(feedUriOrNothing.Value), ex => "error invalid uri format"));
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .MapTry((uri) => new Uri(uri), ex => ex.Message)
            .Map(feedUri => new FeedUri(feedUri));
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

public record NipkgPushAzBlobFeedMetaRequest(AzBlobFeedDefinition AzFeedDefinition, LocalFeedDefinition LocalFeedDefinition) : IRequest<Result>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler(IUploadAzBlobService uploadService)
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }

    private async Task<Result> UploadFileAsync(AzBlobUri uri, LocalFilePath filePath) =>
     await uploadService.UploadFileAsync(uri, filePath);
}

