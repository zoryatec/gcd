using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record LocalDirPath
{
    public static Result<LocalDirPath> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult($"{nameof(LocalDirPath)} should not be empty")
            .Ensure(value => value != string.Empty, $"{nameof(LocalDirPath)} should not be empty")
            .Map(value => new LocalDirPath(value));
    }
    private LocalDirPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}

