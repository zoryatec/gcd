using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Commands.Tools;
public sealed class LabViewProjectPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--project-path";
    public Maybe<string> Map() =>
        Maybe.From(this.Value());
}