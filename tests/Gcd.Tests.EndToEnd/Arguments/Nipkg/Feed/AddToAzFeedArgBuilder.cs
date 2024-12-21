using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;

public class AddToAzFeedArgBuilder : ArgumentsBuilder
{
    public AddToAzFeedArgBuilder()
    {
        WithArg("nipkg");
        WithArg(COMMAND_NAME);
    }

    public AddToAzFeedArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(PACKAGE_PATH_OPTION, value);
        return this;
    }
    public AddToAzFeedArgBuilder WithPackagePath(string value)
    {
        WithOption(PACKAGE_PATH_OPTION, value);
        return this;
    }

    public AddToAzFeedArgBuilder WithAzFeedUri(string value)
    {
        WithOption(AZ_FEED_URI_OPTION, value);
        return this;
    }
}
