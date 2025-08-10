using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedGitHub;

public abstract class ArgBuilderFeedGit<TSelf>(Arguments arguments) : ArgBuilder(arguments)
    where TSelf : ArgBuilderFeedGit<TSelf>
{
    public TSelf WithGitRepoAddressOpt(string value)
    {
        WithOption(GitRepoAddressOption.NAME, value);
        return (TSelf) this;
    }

    public TSelf WithGitBranchNameOpt(string value)
    {
        WithOption(GitBranchNameOption.NAME, value);
        return (TSelf) this;
    }
    public TSelf WithGitUserNameOpt(string value)
    {
        WithOption(GitUserNameOption.NAME, value);
        return (TSelf) this;
    }

    public TSelf WithGitPasswordOpt(string value)
    {
        WithOption(GitPasswordOption.NAME, value);
        return (TSelf) this;
    }

    public TSelf WithGitCommitterNameOpt(string value)
    {
        WithOption(GitCommitterNameOption.NAME, value);
        return (TSelf) this;
    }
    public TSelf WithGitCommitterEmailOpt(string value)
    {
        WithOption(GitCommiterEmailOption.NAME, value);
        return (TSelf) this;
    }
}