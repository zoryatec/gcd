using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedGitHub;

public sealed class ArgBuilderAddLocalPackage
    : ArgBuilderFeedGit<ArgBuilderAddLocalPackage>
{
    
    public ArgBuilderAddLocalPackage(Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-local-package");
    }
    
    public ArgBuilderAddLocalPackage WithPackageLocalPathOpt(string value)
    {
        _arguments.Add(PackageLocalPathOption.NAME, value);
        return  this;
    }
    public ArgBuilderAddLocalPackage WithFeedCreateFlag()
    {
        _arguments.Add(FeedCreateOption.NAME);
        return this;
    }
}