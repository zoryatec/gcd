using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record RelativeDirPath : IRelativeDirPath
{
    public static RelativeDirPath Root => None;
    public static RelativeDirPath None => new (string.Empty);
    public static Result<RelativeDirPath,Error> Of(Maybe<string> maybeValue) =>
        maybeValue.ToResult(ErorNullValue.Of(nameof(LocalDirPath)))
            .Ensure<string, Error>(val => !string.IsNullOrEmpty(val), ErrorEmptyValue.Of(nameof(LocalDirPath)))
            .Map(value => new RelativeDirPath(value));
    private RelativeDirPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}
