using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder.Init;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class NipkgArgBuilder : ArgumentsBuilder
{
    public NipkgArgBuilder()
    {
    }

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

    public NipkgArgBuilder WithAddLocalPackageCmd()
    {
        WithArg("add-local-package");
        return this;
    }

    public NipkgArgBuilder WithPackageLocalPathOpt(string value)
    {
        WithOption(PackageLocalPathOption.NAME, value);
        return this;
    }

    public NipkgArgBuilder WithFeedLocalDirOpt(string value)
    {
        WithOption(FeedLocalDirOption.NAME, value);
        return this;
    }
    public NipkgArgBuilder WithFeedCreateFlag()
    {
        WithArg(FeedCreateOption.NAME);
        return this;
    }
}
