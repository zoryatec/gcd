using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record LocalDirPath : IDirectoryDescriptor
{
    public static Result<LocalDirPath> Parse(Maybe<string> maybeValue)
    {
        var isAbsolute = Path.IsPathFullyQualified(maybeValue.Value);
        if (!isAbsolute)
        {
            string currentDirectoryPath = Environment.CurrentDirectory;
            string packageDirectoryPath = Path.Combine(currentDirectoryPath, maybeValue.Value);
            maybeValue = Maybe.From(packageDirectoryPath);
        }

        var res = maybeValue.ToResult(Error.Of($"{nameof(LocalDirPath)} should not be empty"))
            .Ensure(value => value != string.Empty, Error.Of($"{nameof(LocalDirPath)} should not be empty"))
            .Map(value => new LocalDirPath(value));
        return res.MapError(er => er.Message);
    }
    protected LocalDirPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}
