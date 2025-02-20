using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public sealed record PackageBuilderContentDir : ILocalDirPath
{
    public static Result<PackageBuilderContentDir> Of(BuilderRootDir rootDir, InatallationTargetRootDir packageInstalatioDir)
    {
        var windPath = packageInstalatioDir.Value.Replace('/', '\\');
        var dir = LocalDirPath.Of($"{rootDir.Value}\\data\\{windPath}").MapError(er => er.Message);
        return dir.Map((dir) => new PackageBuilderContentDir(dir));
    }

    private PackageBuilderContentDir(ILocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
    public string Value => DirPath.Value;

    private ILocalDirPath DirPath { get; }
}
