using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackagePath
{
    public static Result<PackagePath> Create(Maybe<string> packagePathOrNothing)
    {
        return packagePathOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "Package path should not be empty")
            .Map(feedUri => new PackagePath(feedUri));
    }

    private PackagePath(string path) => Value = path;
    public string Value { get; }
}



