
using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.FeedLocal.AddPackageLocal;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using McMaster.Extensions.CommandLineUtils;


namespace Gcd.Commands.Nipkg;


public sealed class PackageLocalPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-local-path";
    public Result<PackageFilePath> ToPackageLocalPath() =>
        PackageFilePath.Of(this.Value());
}

public sealed class PackageHttpPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-http-path";
    public Result<PackageHttpPath> ToPackageHttpPath() =>
         PackageHttpPath.Of(this.Value());
}

public sealed class FeedLocalDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--feed-local-path";
    public Result<FeedDefinitionLocal> ToLocalFeedDefinition() =>
                 LocalDirPath.Parse(this.Value())
                    .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));
}

public sealed class GitRepoAddressOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-repo-address";
    public Result<GitRepoAddress> Map() => GitRepoAddress.Of(this.Value());
}

public sealed class GitUserNameOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-user-name";
    public Result<GitUserName> Map() => GitUserName.Of(this.Value());
}

public sealed class GitPasswordOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-user-password";
    public Result<GitPassword> Map() => GitPassword.Of(this.Value());
}

public sealed class GitCommitterNameOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-committer-name";
    public Result<GitCommitterName> Map() => GitCommitterName.Of(this.Value());
}

public sealed class GitCommiterEmailOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-committer-email";
    public Result<GitCommiterEmail> Map() => GitCommiterEmail.Of(this.Value());
}

// flags
public sealed class FeedCreateOption() : CommandOption(NAME, CommandOptionType.NoValue)
{
    public static readonly string NAME = "--feed-create";
    public bool IsSet() =>
        this.HasValue();
}

public sealed class UseAbsolutePathOption() : CommandOption(NAME, CommandOptionType.NoValue)
{
    public static readonly string NAME = "--use-absolute-path";
    public UseAbsolutePath Map()
    {
        if (this.HasValue()) return UseAbsolutePath.Yes;
        else return UseAbsolutePath.No;
    }
}



