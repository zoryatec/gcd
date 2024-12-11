
using Azure.Core;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Services;

namespace Gcd.Model;

public record PackageBuilderDefinition
{
    public LocalDirPath RootDir { get; }
    public LocalDirPath DataDir { get; }
    public LocalDirPath ControlDir { get; }
    public LocalDirPath ContentDir { get; }
    public LocalFilePath DebianFile { get; }
    public LocalFilePath ControlFile { get; }
    public LocalFilePath InstructionFile { get; }

    public static Result<PackageBuilderDefinition> Of(LocalDirPath feedDirPath, PackageInstalationDir packageInstalationDir)
    {
        string windPath = packageInstalationDir.Value.Replace('/', '\\');

        var rootDir = LocalDirPath.Of($"{feedDirPath.Value}");
        var dataDir = LocalDirPath.Of($"{feedDirPath.Value}\\data");
        var controlDir = LocalDirPath.Of($"{feedDirPath.Value}\\control");
        var contentDir = LocalDirPath.Of($"{feedDirPath.Value}\\data\\{windPath}");
        var debianFile = LocalFilePath.Of($"{feedDirPath.Value}\\debian-binary");
        var controlFile = LocalFilePath.Of($"{feedDirPath.Value}\\control\\control");
        var instructionsFile = LocalFilePath.Of($"{feedDirPath.Value}\\data\\instructions");


        return Result
            .Combine(rootDir,dataDir, controlDir, contentDir, debianFile, controlFile, instructionsFile)
            .Map(() => new PackageBuilderDefinition(
                rootDir.Value,
                dataDir.Value,
                controlDir.Value,
                contentDir.Value,
                debianFile.Value,
                controlFile.Value,
                instructionsFile.Value
                ));
    }
    private PackageBuilderDefinition
        (LocalDirPath rootDir,
        LocalDirPath dataDir,
        LocalDirPath controlDir,
        LocalDirPath contentDir,
        LocalFilePath debianFile,
        LocalFilePath controlFile,
        LocalFilePath instrFile)
    {
        RootDir = rootDir;
        DataDir = dataDir;
        ControlDir = controlDir;
        ContentDir = contentDir;
        DebianFile = debianFile;
        ControlFile = controlFile;
        InstructionFile = instrFile;
    }
}
