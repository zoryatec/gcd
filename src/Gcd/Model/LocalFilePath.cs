using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record LocalFilePath
{
    public static Result<LocalFilePath> Offf(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(filepath => Path.Combine(currentDirectoryPath, filepath))
            .Map(feedUri => new LocalFilePath(feedUri));
    }

    protected LocalFilePath(string path) => Value = path;
    public string Value { get; }

    public override string ToString() => Value;
}

