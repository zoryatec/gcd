using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model;

public record AzBlobContainerUri : IDirectoryDescriptor
{
    public static Result<AzBlobContainerUri> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .MapTry((uri) => new Uri(uri), ex => ex.Message)
            .Map(feedUri => new AzBlobContainerUri(feedUri));
    }
    private AzBlobContainerUri(Uri value) => _uri = value;
    private Uri _uri;
    public string Full { get => _uri.AbsoluteUri; }
    public string BaseUri { get => _uri.GetLeftPart(UriPartial.Path); }
    public string Query { get => _uri.Query; }
}


