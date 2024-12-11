using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageBuilderRootDir : LocalDirPath
{
    public static Result<PackageBuilderRootDir> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageBuilderRootDir)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageBuilderRootDir)} should not be empty")
            .Map(feedUri => new PackageBuilderRootDir(feedUri));

    private PackageBuilderRootDir(string path) : base(path) { }

}

