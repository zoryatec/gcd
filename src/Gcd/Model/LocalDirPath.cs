using Azure.Core;
using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record LocalDirPath
{
    public static Result<LocalDirPath> Parse(Maybe<string> maybeValue)
    {
        var isAbsolute = Path.IsPathFullyQualified(maybeValue.Value);
        if (!isAbsolute)
        {
            string currentDirectoryPath = Environment.CurrentDirectory;
            string packageDirectoryPath = Path.Combine(currentDirectoryPath, maybeValue.Value);
            maybeValue =  Maybe.From(packageDirectoryPath);
        }

        return maybeValue.ToResult($"{nameof(LocalDirPath)} should not be empty")
            .Ensure(value => value != string.Empty, $"{nameof(LocalDirPath)} should not be empty")
            .Map(value => new LocalDirPath(value));
    }
    protected LocalDirPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}
