using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.FeedDefinition;

public record SmbFilePath : IFileDescriptor
{
    public static Result<SmbFilePath,Error> Of(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult(Error.Of("FeedUri should not be empty"))
        .Ensure(blobUri => blobUri != string.Empty, Error.Of("FeedUri should not be empty"))
        .Map(blobUri => new SmbFilePath(blobUri));
    }
    private SmbFilePath(string value) => Value = value;

    public string Value { get; }
}



