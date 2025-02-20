using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedSmb;

public abstract class ArgBuilderFeedSmb<TSelf>(Arguments arguments) : ArgBuilder(arguments) 
    where TSelf : ArgBuilderFeedSmb<TSelf>
{
    public TSelf WithSmbShareAddress(string value)
    {
        WithOption(SmbShareAddressOption.NAME, value);
        return (TSelf) this;
    }
    public TSelf WithSmbUserName(string value)
    {
        WithOption(SmbUserNameOption.NAME, value);
        return (TSelf) this;
    }
    public TSelf WithSmbUserPassword(string value)
    {
        WithOption(SmbPasswordOption.NAME, value);
        return (TSelf) this;
    }
}