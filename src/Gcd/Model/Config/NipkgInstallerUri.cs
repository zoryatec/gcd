using CSharpFunctionalExtensions;

namespace Gcd.Model.Config;

public record NipkgInstallerUri : ConfigProperty
{
    public static NipkgInstallerUri None = new NipkgInstallerUri("unset");
    public static Result<NipkgInstallerUri> Of(Maybe<string> uriOrNothing)
    {
        return uriOrNothing.ToResult("uri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "uri should not be empty")
            .Map(feedUri => new NipkgInstallerUri(feedUri));
    }
    private NipkgInstallerUri(string value) :base(value) { }


}

