using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.FeedDefinition;

public record SmbDirPath : IDirectoryDescriptor
{
    public static Result<SmbDirPath> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .Map(feedUri => new SmbDirPath(feedUri));
    }
    private SmbDirPath(string value) => Value = value;

    public string Value { get; }
}



