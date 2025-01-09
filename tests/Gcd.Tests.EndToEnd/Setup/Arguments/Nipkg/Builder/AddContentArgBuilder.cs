using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class AddContentArgBuilder : ArgumentsBuilder
{
    public AddContentArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuBuilderExt.NAME);
        WithArg("add-content");
    }
    public AddContentArgBuilder WithPackageBuilderRootDir(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }

    public AddContentArgBuilder WithContentSourceDir(string value)
    {
        WithOption(BuilderContentSourceDirOption.NAME, value);
        return this;
    }


    public AddContentArgBuilder WithInatallationTargetRootDir(string value)
    {
        WithOption(InatallationTargetRootDirOption.NAME, value);
        return this;
    }
}
