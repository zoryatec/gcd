using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.Model.Nipkg.ControlFile;

public record PackageVersion : ControlFileProperty
{
    public static PackageVersion Default => new PackageVersion("0.0.0.1");
    public static Result<PackageVersion,Error> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult(new Error($"{nameof(PackageVersion)} should not be empty"))
              .Ensure(packagePath => packagePath != string.Empty, new Error($"{nameof(PackageVersion)} should not be empty"))
              .Map(feedUri => new PackageVersion(feedUri));

    private PackageVersion(string path) => Value = path;
    public string Value { get; }
    public static string Key { get; } = "Version";
    public override string ToString() => Value;
}


