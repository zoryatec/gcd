using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record LocalFilePath(ILocalDirPath Directory, IFileName FileName) : IFileDescriptor, ILocalFilePath
{
    public static Result<LocalFilePath,Error> Of(Maybe<string> maybeValue) =>
            from value  in maybeValue.ToResult(ErorNullValue.Of(nameof(LocalFilePath)))
                .Ensure<string, Error>(val => !string.IsNullOrEmpty(val), ErrorEmptyValue.Of(nameof(LocalFilePath)))
            from rawFileName in Result.Success<string, Error>(Path.GetFileName(value))
            from rawRecDir in Result.Success<string, Error>(Path.GetDirectoryName(value))
            from fileName1 in Gcd.LocalFileSystem.Abstractions.FileName.Of(rawFileName)
            from dir in LocalDirPath.Of(rawRecDir)
            select new LocalFilePath(dir, fileName1);
    
    public string Value => Path.Combine(Directory.Value, FileName.Value);
    public override string ToString() => Value;
}

