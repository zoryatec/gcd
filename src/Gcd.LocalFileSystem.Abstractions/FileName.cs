using CSharpFunctionalExtensions;

namespace Gcd.LocalFileSystem.Abstractions;

public  record FileName
{
    public static Result<FileName> Of(Maybe<string> maybeValue) =>
    
        maybeValue
            .ToResult($"{nameof(FileName)} should not be empty")
            .Ensure(value => value != string.Empty, $"{nameof(FileName)} should not be empty")
            .Map(value => new FileName(value));
    
    public FileName(string value) { Value = value; }

    public string Value { get; }
    public override string ToString() => Value;
}

