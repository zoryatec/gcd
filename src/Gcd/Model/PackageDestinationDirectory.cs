using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageDestinationDirectory
{
    public static Result<PackageDestinationDirectory> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(PackageDestinationDirectory)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageDestinationDirectory)} should not be empty")
              .Map(feedUri => new PackageDestinationDirectory(feedUri));

    private PackageDestinationDirectory(string path) => Value = path;
    public string Value { get; }
}

