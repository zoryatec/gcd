using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedSmb;

public sealed class ArgBuilderPushMetaData
    : ArgBuilderFeedSmb<ArgBuilderPushMetaData>
{
    public ArgBuilderPushMetaData (Arguments arguments) : base(arguments)
    {
        _arguments.Add("push-meta-data");
    }

    public ArgBuilderPushMetaData WithFeedLocalDirOpt(string value)
    {
        _arguments.Add(FeedLocalDirOption.NAME, value);
        return this;
    }
}