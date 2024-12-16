using CSharpFunctionalExtensions;

namespace Gcd.Model.Config;

public record NipkgInstallerUri
{
    public static Result<NipkgInstallerUri> Of(Maybe<string> uriOrNothing)
    {
        return uriOrNothing.ToResult("uri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "uri should not be empty")
            .MapTry((uri) => new Uri(uri), ex => ex.Message)
            .Map(feedUri => new NipkgInstallerUri(feedUri));
    }
    private NipkgInstallerUri(Uri value) => _uri = value;
    private Uri _uri;
    public string Value { get => _uri.AbsoluteUri; }

}

