using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Commands.Tools;
public sealed class DownloadNipkgPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--download-path";
    public Result<LocalFilePath> Map() =>
        LocalFilePath.Of(this.Value()).MapError(er => er.Message);
}

public sealed class NipkgInstallerSourceUrlOption : CommandOption
{
    public static readonly string NAME = "--installer-source-uri";
    public static readonly string DESCRIPTION = "The URL of the NIPM/NIPKG installer.";
    public NipkgInstallerSourceUrlOption() : base (NAME, CommandOptionType.SingleValue)
    {
        Description = DESCRIPTION;
    }

    public Result<NipkgInstallerUri> Map() =>
        NipkgInstallerUri.Of(this.Value());
}

public sealed class DownloadArchiveDestinationDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--destination-dir";

    public Result<LocalDirPath, Error> Map() =>
        LocalDirPath.Of(this.Value());
}

public sealed class DownloadArchiveRelativeDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--archive-relative-dir";

    public Result<RelativeDirPath, Error> Map() =>
        RelativeDirPath.Of(this.Value());
}

public sealed class DownloadArchiveSourceUriOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--source-uri";

    public Result<WebFileUri, Error> Map() =>
        WebFileUri.Of(this.Value()).MapError(er => new Error(er));
}