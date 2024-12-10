using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageContentDir
{
    public static Result<PackageContentDir> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageContentDir)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageContentDir)} should not be empty")
            .Map(feedUri => new PackageContentDir(feedUri));

    private PackageContentDir(string path) => Value = path;
    public string Value { get; }
}

