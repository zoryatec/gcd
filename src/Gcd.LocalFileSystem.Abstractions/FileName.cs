using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public  record FileName
{
    public static Result<FileName,Error> Of(Maybe<string> maybeValue) =>
    
        maybeValue
            .ToResult(Error.Of($"{nameof(FileName)} should not be null"))
            .Ensure(value => value != string.Empty, Error.Of($"{nameof(FileName)} should not be empty"))
            .Map(value => new FileName(value));
    
    public FileName(string value) { Value = value; }

    public string Value { get; }
    public override string ToString() => Value;
}

