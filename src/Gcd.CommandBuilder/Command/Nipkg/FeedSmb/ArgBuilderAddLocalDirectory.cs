using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedSmb;

public sealed class ArgBuilderAddLocalDirectory
    : ArgBuilderFeedSmb<ArgBuilderAddLocalDirectory>
{
    
    public ArgBuilderAddLocalDirectory(Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-local-directory");
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