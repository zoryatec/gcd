using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.Builder;

public class ArgBuilderAddContent : ArgBuilder
{
    public ArgBuilderAddContent(Arguments arguments) : base(arguments)
    {
        WithArg("add-content");
    }
    public ArgBuilderAddContent WithPackageBuilderRootDir(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }

    public ArgBuilderAddContent WithContentSourceDir(string value)
    {
        WithOption(BuilderContentSourceDirOption.NAME, value);
        return this;
    }


    public ArgBuilderAddContent WithInatallationTargetRootDir(string value)
    {
        WithOption(InatallationTargetRootDirOption.NAME, value);
        return this;
    }
}
