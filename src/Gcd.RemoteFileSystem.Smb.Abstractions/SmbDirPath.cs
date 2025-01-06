using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.FeedDefinition;

public record SmbDirPath : IDirectoryDescriptor
{
    public static Result<SmbDirPath, Error> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult(Error.Of("FeedUri should not be empty"))
            .Ensure(feedUri => feedUri != string.Empty, Error.Of("FeedUri should not be empty"))
            .Map(feedUri => new SmbDirPath(feedUri));
    }
    private SmbDirPath(string value) => Value = value;

    public string Value { get; }
}



