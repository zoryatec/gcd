using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.RemoteFileSystem.Rclone.Abstractions;

public record RcloneDirPath : IDirectoryDescriptor
{
    public static Result<RcloneDirPath, Error> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult(new Error("FeedUri should not be empty"))
            .Ensure(feedUri => feedUri != string.Empty, new Error("FeedUri should not be empty"))
            .Map(feedUri => new RcloneDirPath(feedUri));
    }
    private RcloneDirPath(string value) => Value = value;

    public string Value { get; }
}



