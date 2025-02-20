using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedRclone;

public abstract class ArgBuilderFeedRclone<TSelf>(Arguments arguments) : ArgBuilder(arguments) 
    where TSelf : ArgBuilderFeedRclone<TSelf>
{
    public TSelf WithRcloneFeedDirOption(string value)
    {
        WithOption(RcloneFeedDirOption.NAME, value);
        return (TSelf) this;
    }
}