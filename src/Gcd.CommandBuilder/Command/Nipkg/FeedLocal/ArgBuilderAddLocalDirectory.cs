using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedLocal;

public sealed class ArgBuilderAddLocalDirectory
    : ArgBuilderFeedLocal<ArgBuilderAddLocalDirectory>
{
    
    public ArgBuilderAddLocalDirectory(Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-local-directory");
    }
    
    public ArgBuilderAddLocalDirectory WithPackageLocalPathOpt(string value)
    {
        _arguments.Add(PackageLocalPathOption.NAME, value);
        return  this;
    }
    public ArgBuilderAddLocalDirectory WithFeedCreateFlag()
    {
        _arguments.Add(FeedCreateOption.NAME);
        return this;
    }
    
    public ArgBuilderAddLocalDirectory WithPackageLocalDirOpt(string value)
    {
        WithOption(PackageLocalDirectoryOption.NAME, value);
        return this;
    }
}