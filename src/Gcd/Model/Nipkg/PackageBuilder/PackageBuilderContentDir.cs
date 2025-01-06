using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public sealed record PackageBuilderContentDir : LocalDirPath
{
    public static Result<PackageBuilderContentDir> Of(BuilderRootDir rootDir, InatallationTargetRootDir packageInstalatioDir)
    {
        var windPath = packageInstalatioDir.Value.Replace('/', '\\');
        var dir = Parse($"{rootDir.Value}\\data\\{windPath}").MapError(er => er.Message);
        return dir.Map((dir) => new PackageBuilderContentDir(dir));
    }
    private PackageBuilderContentDir(LocalDirPath value) : base(value) { }
}
