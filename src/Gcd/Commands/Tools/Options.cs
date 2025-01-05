using CSharpFunctionalExtensions;
using Gcd.Model.File;
using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Commands.Tools;
public sealed class DownloadNipkgPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--download-path";
    public Result<LocalFilePath> Map() =>
        LocalFilePath.Offf(this.Value());
}