using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record AzBlobFeedUri
{
    public static Result<AzBlobFeedUri> Create(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .MapTry((uri) => new Uri(uri), ex => ex.Message)
            .Map(feedUri => new AzBlobFeedUri(feedUri));
    }
    private AzBlobFeedUri(Uri value) => _uri = value;
    private Uri _uri;
    public string Full { get => _uri.AbsoluteUri; }
    public string BaseUri { get => _uri.GetLeftPart(UriPartial.Path); }
    public string Query { get => _uri.Query; }
}


