using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record BuilderRootDir : ILocalDirPath
{
    public string Value => DirPath.Value;

    public LocalDirPath DirPath { get; }

    public static Result<BuilderRootDir> Of(Maybe<string> packagePathOrNothing) =>
        LocalDirPath.Parse(packagePathOrNothing).MapError(er => er.Message)
        .Map(x => new BuilderRootDir(x));
    private BuilderRootDir(LocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
}

