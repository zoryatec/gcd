using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedGitHub;

public sealed class ArgBuilderPullMetaData
    : FeedGit.ArgBuilderFeedGit<ArgBuilderPullMetaData>
{
    public ArgBuilderPullMetaData (Arguments arguments) : base(arguments)
    {
        _arguments.Add("pull-meta-data");
    }

    public ArgBuilderPullMetaData WithFeedLocalDirOpt(string value)
    {
        _arguments.Add(FeedLocalDirOption.NAME, value);
        return this;
    }
}