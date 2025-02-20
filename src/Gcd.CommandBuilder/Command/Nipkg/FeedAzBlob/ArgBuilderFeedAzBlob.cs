using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob;

public abstract class ArgBuilderFeedAzBlob<TSelf>(Arguments arguments) : ArgBuilder(arguments)
    where TSelf : ArgBuilderFeedAzBlob<TSelf>
{
    public TSelf WithAzFeedUriOpt(string value)
    {
        WithOption(AzFeedUrlOption.NAME, value);
        return (TSelf) this;
    }
}