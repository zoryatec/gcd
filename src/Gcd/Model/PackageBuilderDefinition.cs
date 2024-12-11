
using Azure.Core;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Services;

namespace Gcd.Model;


public record PackageBuilderContentDir
{
    public LocalDirPath Value { get; }

    public static Result<PackageBuilderContentDir> Of(PackageBuilderRootDir rootDir, PackageInstalationDir packageInstalatioDir)
    {
        string windPath = packageInstalatioDir.Value.Replace('/', '\\');
        var dir = LocalDirPath.Of($"{rootDir.Value}");
        return dir.Map((dir) => new PackageBuilderContentDir(dir));
    }
    private PackageBuilderContentDir(LocalDirPath value)
    {
        Value = value;
    }
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


        var rootDir = LocalDirPath.Of($"{feedDirPath.Value}");
        var dataDir = LocalDirPath.Of($"{feedDirPath.Value}\\data");
        var controlDir = LocalDirPath.Of($"{feedDirPath.Value}\\control");

        var debianFile = LocalFilePath.Of($"{feedDirPath.Value}\\debian-binary");
        var controlFile = LocalFilePath.Of($"{feedDirPath.Value}\\control\\control");
        var instructionsFile = LocalFilePath.Of($"{feedDirPath.Value}\\data\\instructions");


        return Result
            .Combine(rootDir,dataDir, controlDir, debianFile, controlFile, instructionsFile)
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
