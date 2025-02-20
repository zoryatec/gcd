using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record LocalDirPath : IDirectoryDescriptor, ILocalDirPath
{
    public static Result<LocalDirPath,Error> Of(Maybe<string> maybeValue) =>
        maybeValue.ToResult(ErorNullValue.Of(nameof(LocalDirPath)))
            .Ensure<string, Error>(val => !string.IsNullOrEmpty(val), ErrorEmptyValue.Of(nameof(LocalDirPath)))
            .BindIf(val => !Path.IsPathFullyQualified(val), ConvertToAbsolutePath)
            .Map(value => new LocalDirPath(value));

    private static Result<string, Error> ConvertToAbsolutePath(string relativePath)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string absolutePath = Path.Combine(currentDirectoryPath, relativePath);
        return Result.Success<string, Error>(absolutePath);
    }

    private LocalDirPath(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}
