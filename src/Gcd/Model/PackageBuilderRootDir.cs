using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageBuilderRootDir : LocalDirPath
{
    public static Result<PackageBuilderRootDir> Create(Maybe<string> packagePathOrNothing) =>
        LocalDirPath.Parse(packagePathOrNothing)
        .Map(x => new PackageBuilderRootDir(x.Value));
    private PackageBuilderRootDir(string path) : base(path) { }
}

