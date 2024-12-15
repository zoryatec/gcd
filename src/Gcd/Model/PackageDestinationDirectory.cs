using CSharpFunctionalExtensions;
using Gcd.Model.File;

namespace Gcd.Model;

public record PackageDestinationDirectory : LocalDirPath
{
    public static Result<PackageDestinationDirectory> Of(Maybe<string> maybeValue) =>
        LocalDirPath.Parse(maybeValue)
        .Map(x => new PackageDestinationDirectory(x));

    private PackageDestinationDirectory(LocalDirPath value) : base(value) { }
}

