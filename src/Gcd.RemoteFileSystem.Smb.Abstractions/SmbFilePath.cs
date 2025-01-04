using CSharpFunctionalExtensions;
using Gcd.Model.File;

namespace Gcd.Model.FeedDefinition;

public record SmbFilePath : IFileDescriptor
{
    public static Result<SmbFilePath> Of(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult("FeedUri should not be empty")
        .Ensure(blobUri => blobUri != string.Empty, "FeedUri should not be empty")
        .Map(blobUri => new SmbFilePath(blobUri));
    }
    private SmbFilePath(string value) => Value = value;

    public string Value { get; }
}



