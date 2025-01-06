using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.Common;

public record PackageDestinationDirectory : ILocalDirPath
{
    public static Result<PackageDestinationDirectory> Of(Maybe<string> maybeValue) =>
        LocalDirPath.Of(maybeValue).MapError(er => er.Message)
        .Map(x => new PackageDestinationDirectory(x));


    private PackageDestinationDirectory(ILocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
    public string Value => DirPath.Value;

    private ILocalDirPath DirPath { get; }
}

