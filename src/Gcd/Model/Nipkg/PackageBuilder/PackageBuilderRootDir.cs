using CSharpFunctionalExtensions;
using Gcd.Model.File;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record PackageBuilderRootDir : LocalDirPath
{
    public static Result<PackageBuilderRootDir> Of(Maybe<string> packagePathOrNothing) =>
        Parse(packagePathOrNothing)
        .Map(x => new PackageBuilderRootDir(x));
    private PackageBuilderRootDir(LocalDirPath value) : base(value) { }
}

