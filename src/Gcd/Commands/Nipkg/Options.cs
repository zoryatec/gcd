
using CSharpFunctionalExtensions;
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

public sealed class FeedCreateOption() : CommandOption(NAME, CommandOptionType.NoValue)
{
    public static readonly string NAME = "--feed-create";
    public bool IsSet() =>
        this.HasValue();
}