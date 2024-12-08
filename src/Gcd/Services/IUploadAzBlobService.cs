using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

public record AzBlobUri
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

public record FilePath
{
    public static Result<FilePath> Create(Maybe<string> packagePathOrNothing)
    {
        return packagePathOrNothing.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(feedUri => new FilePath(feedUri));
    }

    private FilePath(string path) => Value = path;
    public string Value { get; }
}
public interface IUploadAzBlobService
{
    public Task<Result> UploadFileAsync(AzBlobUri blobUri, FilePath filePath );
}

