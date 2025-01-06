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
    public static Result<LocalFilePath> Offf(Maybe<string> maybeValue){
        var res =
            from value  in maybeValue.ToResult(Error.Of("path should not be null"))
            from currDir in Result.Success<string,Error>(Environment.CurrentDirectory)
            from rawFileName in Result.Success<string, Error>(Path.GetFileName(value))
            from rawRecDir in Result.Success<string, Error>(Path.GetDirectoryName(value))
            from fileName1 in FileName.Of(rawFileName)
            from rawDir in Result.Success<string, Error>(Path.Combine(currDir, rawRecDir))
            from dir in LocalDirPath.Parse(rawDir)
            select new LocalFilePath(dir, fileName1);

        return res.MapError(er => er.Message); }

    //protected LocalFilePath(string path) => Value = path;
    public string Value => Path.Combine(Directory.Value, FileName.Value);


    public override string ToString() => Value;

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }
}

