using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageBuilderRootDir : LocalDirPath
{
    public static Result<PackageBuilderRootDir> Of(Maybe<string> packagePathOrNothing) =>
        LocalDirPath.Parse(packagePathOrNothing)
        .Map(x => new PackageBuilderRootDir(x));
    private PackageBuilderRootDir(LocalDirPath value) : base(value) { }
}

