using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder.Init;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class NipkgArgBuilder : ArgumentsBuilder
{
    public NipkgArgBuilder()
    {
    }


    // cmd
    public NipkgArgBuilder WithNipkgCmd()
    {
        WithArg("nipkg");
        return this;
    }

    public NipkgArgBuilder WithFeedLocalCmd()
    {
        WithArg("feed-local");
        return this;
    }

    public NipkgArgBuilder WithFeedGitCmd()
    {
        WithArg("feed-git");
        return this;
    }

    public NipkgArgBuilder WithAddLocalPackageCmd()
    {
        WithArg("add-local-package");
        return this;
    }

    public NipkgArgBuilder WithAddHttpPackageCmd()
    {
        WithArg("add-http-package");
        return this;
    }


    // options
    #region git
    public NipkgArgBuilder WithGitRepoAddressOpt(string value)
    {
        WithOption(GitRepoAddressOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithGitBranchNameOpt(string value)
    {
        WithOption(GitBranchNameOption.NAME, value);
        return this;
    }
    public NipkgArgBuilder WithGitUserNameOpt(string value)
    {
        WithOption(GitUserNameOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithGitPasswordOpt(string value)
    {
        WithOption(GitPasswordOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithGitCommitterNameOpt(string value)
    {
        WithOption(GitCommitterNameOption.NAME, value);
        return this;
    }
    public NipkgArgBuilder WithGitCommitterEmailOpt(string value)
    {
        WithOption(GitCommiterEmailOption.NAME, value);
        return this;
    }

    #endregion
    #region local
    public NipkgArgBuilder WithPackageLocalPathOpt(string value)
    {
        WithOption(PackageLocalPathOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithPackageHttpOpt(string value)
    {
        WithOption(PackageHttpPathOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithFeedLocalDirOpt(string value)
    {
        WithOption(FeedLocalDirOption.NAME, value);
        return this;
    }
    #endregion

    // flags
    public NipkgArgBuilder WithFeedCreateFlag()
    {
        WithArg(FeedCreateOption.NAME);
        return this;
    }
    public NipkgArgBuilder WithUseAbsolutePathFlag()
    {
        WithArg(UseAbsolutePathOption.NAME);
        return this;
    }
}
