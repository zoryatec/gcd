using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.FeedAzBlob;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;

public class AddToAzFeedArgBuilder : ArgumentsBuilder
{
    public AddToAzFeedArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuFeedAzBlobExt.NAME);
        WithArg(UseCmdAddLocalPackageExt.NAME);
    }

    public AddToAzFeedArgBuilder WithPackagePath(string value)
    {
        WithOption(PackageLocalPathOption.NAME, value);
        return this;
    }

    public AddToAzFeedArgBuilder WithAzFeedUri(string value)
    {
        WithOption(AZ_FEED_URI_OPTION, value);
        return this;
    }
}
