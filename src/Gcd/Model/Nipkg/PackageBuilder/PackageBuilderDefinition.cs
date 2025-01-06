
using Azure.Core;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.Nipkg.FeedDefinition;

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

public record PackageBuilderDefinition
{
    public LocalDirPath RootDir { get; }
    public LocalDirPath DataDir { get; }
    public LocalDirPath ControlDir { get; }
    //public LocalDirPath ContentDir { get; }
    public LocalFilePath DebianFile { get; }
    public LocalFilePath ControlFile { get; }
    public LocalFilePath InstructionFile { get; }

    public static Result<PackageBuilderDefinition> Of(LocalDirPath feedDirPath)
    {

        var result =
            from rootDir in LocalDirPath.Parse($"{feedDirPath.Value}")
            from dataDir in LocalDirPath.Parse($"{feedDirPath.Value}\\data")
            from controlDir in LocalDirPath.Parse($"{feedDirPath.Value}\\control")
            from debianFile in LocalFilePath.Offf($"{feedDirPath.Value}\\debian-binary").MapError(er => Error.Of(er))
            from controlFile in LocalFilePath.Offf($"{feedDirPath.Value}\\control\\control").MapError(er => Error.Of(er))
            from instructionsFile in LocalFilePath.Offf($"{feedDirPath.Value}\\data\\instructions").MapError(er => Error.Of(er))
            select new  PackageBuilderDefinition(
                        rootDir,
                        dataDir,
                        controlDir,
                        debianFile,
                        controlFile,
                        instructionsFile);

        return result.MapError(er => er.Message);

    }
    private PackageBuilderDefinition
        (LocalDirPath rootDir,
        LocalDirPath dataDir,
        LocalDirPath controlDir,
        LocalFilePath debianFile,
        LocalFilePath controlFile,
        LocalFilePath instrFile)
    {
        RootDir = rootDir;
        DataDir = dataDir;
        ControlDir = controlDir;
        //ContentDir = contentDir;
        DebianFile = debianFile;
        ControlFile = controlFile;
        InstructionFile = instrFile;
    }
}
