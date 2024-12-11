using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageContentSourceDir
{
    public static Result<PackageContentSourceDir> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageContentSourceDir)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageContentSourceDir)} should not be empty")
            .Map(feedUri => new PackageContentSourceDir(feedUri));

    private PackageContentSourceDir(string path) => Value = path;
    public string Value { get; }
}

