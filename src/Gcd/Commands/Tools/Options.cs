using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Commands.Tools;
public sealed class DownloadNipkgPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--download-path";
    public Result<LocalFilePath> Map() =>
        LocalFilePath.Of(this.Value()).MapError(er => er.Message);
}