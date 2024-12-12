using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageVersion : ControlFileProperty
{
    public static PackageVersion Default => new PackageVersion("0.0.0.1");
    public static Result<PackageVersion> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(PackageVersion)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageVersion)} should not be empty")
              .Map(feedUri => new PackageVersion(feedUri));

    private PackageVersion(string path) => Value = path;
    public string Value { get; }
    public static string Key { get; } = "Version";
    public override string ToString() => Value;
}


