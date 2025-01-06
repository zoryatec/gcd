using CSharpFunctionalExtensions;

namespace Gcd.LocalFileSystem.Abstractions;

public record LocalFilePath : IFileDescriptor, ILocalFilePath
{
    public LocalFilePath(LocalDirPath directory, FileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }
public static Result<LocalFilePath> Offf(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        var recivedDir = Path.GetDirectoryName(maybeValue.Value);
        var fileName = Path.GetFileName(maybeValue.Value);
        var dir = Path.Combine(currentDirectoryPath, recivedDir);


        var fileNameR = new FileName(fileName);

        var locDir = LocalDirPath.Parse(dir); 

        return Result.Success(new LocalFilePath(locDir.Value, fileNameR));

        //return maybeValue.ToResult("FilePath should not be empty")
        //    .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
        //    .Map(filepath => Path.Combine(currentDirectoryPath, filepath))
        //    .Map(feedUri => new LocalFilePath(feedUri));
    }

    //protected LocalFilePath(string path) => Value = path;
    public string Value => Path.Combine(Directory.Value, FileName.Value);


    public override string ToString() => Value;

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }
}

