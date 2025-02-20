using Gcd.Commands.Nipkg;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.FeedRclone;

public sealed class ArgBuilderAddLocalDirectory
    : ArgBuilderFeedRclone<ArgBuilderAddLocalDirectory>
{
    
    public ArgBuilderAddLocalDirectory(Arguments arguments) : base(arguments)
    {
        _arguments.Add("add-local-directory");
    }
    
    public ArgBuilderAddLocalDirectory WithPackageLocalDirOpt(string value)
    {
        WithOption(PackageLocalDirectoryOption.NAME, value);
        return this;
    }
    
    public ArgBuilderAddLocalDirectory WithFeedCreateFlag()
    {
        _arguments.Add(FeedCreateOption.NAME);
        return this;
    }
}