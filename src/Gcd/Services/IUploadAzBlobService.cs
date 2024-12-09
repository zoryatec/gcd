using Azure.Core;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Common;
using MediatR;
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

public record LocalFilePath
{
    public static Result<LocalFilePath> Of(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(filepath => Path.Combine(currentDirectoryPath, filepath))
            .Map(feedUri => new LocalFilePath(feedUri));
    }

    private LocalFilePath(string path) => Value = path;
    public string Value { get; }
}
public interface IUploadAzBlobService
{
    public Task<Result> UploadFileAsync(AzBlobUri blobUri, LocalFilePath filePath );
}

