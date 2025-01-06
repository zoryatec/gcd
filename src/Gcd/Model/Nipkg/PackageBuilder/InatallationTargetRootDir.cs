using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record InatallationTargetRootDir
{
    public static Result<InatallationTargetRootDir> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(InatallationTargetRootDir)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(InatallationTargetRootDir)} should not be empty")
              .Map(feedUri => new InatallationTargetRootDir(feedUri));

    private InatallationTargetRootDir(string path) => Value = path;
    public string Value { get; }
}

