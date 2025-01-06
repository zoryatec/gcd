using CSharpFunctionalExtensions;

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
            from currDir in Result.Success(Environment.CurrentDirectory)
            from rawFileName in Result.Success(Path.GetFileName(maybeValue.Value))
            from rawRecDir in Result.Success(Path.GetDirectoryName(maybeValue.Value))
            from fileName1 in FileName.Of(rawFileName)
            from rawDir in Result.Success(Path.Combine(currDir, rawRecDir))
            from dir in LocalDirPath.Parse(rawDir)
            select new LocalFilePath(dir, fileName1);

        return res; }

    //protected LocalFilePath(string path) => Value = path;
    public string Value => Path.Combine(Directory.Value, FileName.Value);


    public override string ToString() => Value;

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }
}

