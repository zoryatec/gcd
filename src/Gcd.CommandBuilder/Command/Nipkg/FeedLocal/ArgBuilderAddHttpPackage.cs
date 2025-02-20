using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedLocal;

public sealed class ArgBuilderAddHttpPackage
    : ArgBuilderFeedLocal<ArgBuilderAddHttpPackage>
{
    
    public ArgBuilderAddHttpPackage(Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-http-package");
    }
    
    public ArgBuilderAddHttpPackage WithPackageHttpOpt(string value)
    {
        WithOption(PackageHttpPathOption.NAME, value);
        return this;
    }
    
    public ArgBuilderAddHttpPackage WithFeedCreateFlag()
    {
        _arguments.Add(FeedCreateOption.NAME);
        return this;
    }
    
    public ArgBuilderAddHttpPackage WithUseAbsolutePathFlag()
    {
        WithArg(UseAbsolutePathOption.NAME);
        return this;
    }
}