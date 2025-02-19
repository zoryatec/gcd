using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedLocal;

public abstract class ArgBuilderFeedLocal<TSelf>(Arguments arguments) : ArgBuilder(arguments) 
    where TSelf : ArgBuilderFeedLocal<TSelf>
{
    public TSelf WithFeedLocalDirOpt(string value)
    {
        _arguments.Add(FeedLocalDirOption.NAME, value);
        return (TSelf) this;
    }
}