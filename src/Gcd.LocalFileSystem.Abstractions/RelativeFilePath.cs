using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record RelativeFilePath(IRelativeDirPath Directory, IFileName FileName) : IRelativeFilePath
{
    public static Result<RelativeFilePath,Error> Of(Maybe<string> maybeValue) =>
            from value  in maybeValue.ToResult(ErorNullValue.Of(nameof(RelativeFilePath)))
                .Ensure<string, Error>(val => !string.IsNullOrEmpty(val), ErrorEmptyValue.Of(nameof(LocalFilePath)))
            from rawFileName in Result.Success<string, Error>(Path.GetFileName(value))
            from rawRecDir in Result.Success<string, Error>(Path.GetDirectoryName(value))
            from fileName1 in Gcd.LocalFileSystem.Abstractions.FileName.Of(rawFileName)
            from dir in RelativeDirPath.Of(rawRecDir)
            select new RelativeFilePath(dir, fileName1);
    
    public string Value => $"{FileName.Value}"; // completly wrong made just to work with git path
    public override string ToString() => Value;
}

