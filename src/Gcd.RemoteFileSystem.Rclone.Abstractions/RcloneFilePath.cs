using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.RemoteFileSystem.Rclone.Abstractions;

public record RcloneFilePath : IFileDescriptor
{
    public static Result<RcloneFilePath,Error> Of(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult(Error.Of("FeedUri should not be empty"))
        .Ensure(blobUri => blobUri != string.Empty, Error.Of("FeedUri should not be empty"))
        .Map(blobUri => new RcloneFilePath(blobUri));
    }
    private RcloneFilePath(string value) => Value = value;

    public string Value { get; }
}



