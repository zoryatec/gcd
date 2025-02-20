using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedSmb;

public sealed class ArgBuilderAddLocalPackage
    : ArgBuilderFeedSmb<ArgBuilderAddLocalPackage>
{
    public ArgBuilderAddLocalPackage (Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-local-package");
    }
    public ArgBuilderAddLocalPackage WithPackageLocalPathOpt(string value)
    {
        _arguments.Add(PackageLocalPathOption.NAME, value);
        return this;
    }

    public ArgBuilderAddLocalPackage WithFeedLocalDirOpt(string value)
    {
        _arguments.Add(FeedLocalDirOption.NAME, value);
        return this;
    }
    
    public ArgBuilderAddLocalPackage WithFeedCreateFlag()
    {
        _arguments.Add(FeedCreateOption.NAME);
        return this;
    }
}