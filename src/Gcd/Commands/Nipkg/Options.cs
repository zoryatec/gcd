
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.RemoteFileSystem.Rclone.Abstractions;
using McMaster.Extensions.CommandLineUtils;


namespace Gcd.Commands.Nipkg;

#region builer



public sealed class PackageDestinationDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-destination-dir";
    public Result<PackageDestinationDirectory> Map() =>
        PackageDestinationDirectory.Of(this.Value());
}

public sealed class BuilderRootDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-builder-dir";
    public Result<BuilderRootDir> Map() =>
        BuilderRootDir.Of(this.Value());
}


public sealed class InstructionFileSourceOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--instructions-file-path";
    public Maybe<Result<LocalFilePath, Error>> Map()
    {
        var result = Maybe<Result<LocalFilePath,Error>>.None;
        if(this.HasValue())
        {
            return LocalFilePath.Of(this.Value());
        }

        return result;
    }
}

public sealed class ControlFileSourceOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--control-file-path";
    public Maybe<Result<LocalFilePath, Error>> Map()
    {
        var result = Maybe<Result<LocalFilePath, Error>>.None;
        if (this.HasValue())
        {
            return LocalFilePath.Of(this.Value());
        }

        return result;
    }
}








#endregion
#region package source
public sealed class PackageLocalPathOption : CommandOption
{
    public PackageLocalPathOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to local package file. File must have *.nipkg extension.";
    }
    public static readonly string NAME = "--package-local-path";
    public Result<PackageLocalFilePath> ToPackageLocalPath() =>
        PackageLocalFilePath.Of(this.Value()).MapError(er => er.Message);
}

public sealed class PackageLocalDirectoryOption : CommandOption
{
    public PackageLocalDirectoryOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to directory containing nipkg packages.";
    }
    public static readonly string NAME = "--package-local-directory";
    public Result<LocalDirPath,Error> Map() =>
        LocalDirPath.Of(this.Value());
}

public sealed class PackageHttpPathOption : CommandOption
{
    public PackageHttpPathOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to http/https package file. File must have *.nipkg extension. Package must be accessible without additional authentication.";
    }
    public static readonly string NAME = "--package-http-path";
    public Result<PackageHttpPath> ToPackageHttpPath() =>
         PackageHttpPath.Of(this.Value());
}
#endregion 
#region smb
public sealed class SmbUserNameOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--smb-user-name";
    public Result<SmbUserName> Map() => SmbUserName.Of(this.Value());
}

public sealed class SmbPasswordOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--smb-user-password";
    public Result<SmbPassword> Map() => SmbPassword.Of(this.Value());
}

public sealed class SmbShareAddressOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--smb-share-address";
    public Result<SmbShareAddress> Map() => SmbShareAddress.Of(this.Value());
}


#endregion
#region local
public sealed class FeedLocalDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--feed-local-path";
    public Result<FeedDefinitionLocal> ToLocalFeedDefinition() =>
                 LocalDirPath.Of(this.Value()).MapError(er => er.Message)
                    .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));
}
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
#endregion

#region az
public sealed class AzFeedUrlOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--feed-url";
    public Result<AzBlobFeedUri> Map() => AzBlobFeedUri.Create(this.Value());
}

#endregion

#region rclone
public sealed class RcloneFeedDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--rclone-feed-dir";
    public Result<RcloneDirPath,Error> Map() => RcloneDirPath.Of(this.Value());
}
#endregion

#region git

public sealed class GitRepoAddressOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-repo-address";
    public Result<GitRepoAddress> Map() => GitRepoAddress.Of(this.Value());
}

public sealed class GitBranchNameOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--git-branch-name";
    public Result<GitLocalBranch> Map() => GitLocalBranch.Of(this.Value());
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
#endregion



