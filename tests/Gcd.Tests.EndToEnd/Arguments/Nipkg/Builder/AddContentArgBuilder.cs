using Gcd.Commands.Nipkg.Builder.Init;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class AddContentArgBuilder : ArgumentsBuilder
{
    public AddContentArgBuilder()
    {
        WithArg("nipkg");
        WithArg("package-builder");
        WithArg("add-content");
    }
    public AddContentArgBuilder WithPackageBuilderRootDir(string value)
    {
        WithArg(value); ;
        //WithOption(PackageBuilderRootDirOption.NAME, value);
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
