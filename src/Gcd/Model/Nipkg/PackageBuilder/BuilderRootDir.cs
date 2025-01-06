using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record BuilderRootDir : ILocalDirPath
{
    public static Result<BuilderRootDir> Of(Maybe<string> packagePathOrNothing) =>
        LocalDirPath.Of(packagePathOrNothing).MapError(er => er.Message)
        .Map(x => new BuilderRootDir(x));
    private BuilderRootDir(LocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
    public string Value => DirPath.Value;

    private LocalDirPath DirPath { get; }
}

