using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public record LocalFilePath : IFileDescriptor, ILocalFilePath
{
    public LocalFilePath(LocalDirPath directory, FileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }
    public static Result<LocalFilePath,Error> Of(Maybe<string> maybeValue){
        var res =
            from value  in maybeValue.ToResult(ErorNullValue.Of(nameof(LocalFilePath)))
            from rawFileName in Result.Success<string, Error>(Path.GetFileName(value))
            from rawRecDir in Result.Success<string, Error>(Path.GetDirectoryName(value))
            from fileName1 in FileName.Of(rawFileName)
            from dir in LocalDirPath.Of(rawRecDir)
            select new LocalFilePath(dir, fileName1);

        //return res.MapError(er => er.Message); }
        return res;}


    //protected LocalFilePath(string path) => Value = path;
    public string Value => Path.Combine(Directory.Value, FileName.Value);


    public override string ToString() => Value;

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }
}

