using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageInstalationDir
{
    public static Result<PackageInstalationDir> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(PackageInstalationDir)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageInstalationDir)} should not be empty")
              .Map(feedUri => new PackageInstalationDir(feedUri));

    private PackageInstalationDir(string path) => Value = path;
    public string Value { get; }
}

