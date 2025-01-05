
using Azure.Core;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;


public sealed record PackageBuilderContentDir : LocalDirPath
{
    public static Result<PackageBuilderContentDir> Of(BuilderRootDir rootDir, InatallationTargetRootDir packageInstalatioDir)
    {
        var windPath = packageInstalatioDir.Value.Replace('/', '\\');
        var dir = Parse($"{rootDir.Value}\\data\\{windPath}");
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


        var rootDir = LocalDirPath.Parse($"{feedDirPath.Value}");
        var dataDir = LocalDirPath.Parse($"{feedDirPath.Value}\\data");
        var controlDir = LocalDirPath.Parse($"{feedDirPath.Value}\\control");

        var debianFile = LocalFilePath.Offf($"{feedDirPath.Value}\\debian-binary");
        var controlFile = LocalFilePath.Offf($"{feedDirPath.Value}\\control\\control");
        var instructionsFile = LocalFilePath.Offf($"{feedDirPath.Value}\\data\\instructions");


        return Result
            .Combine(rootDir, dataDir, controlDir, debianFile, controlFile, instructionsFile)
            .Map(() => new PackageBuilderDefinition(
                rootDir.Value,
                dataDir.Value,
                controlDir.Value,
                debianFile.Value,
                controlFile.Value,
                instructionsFile.Value
                ));
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
