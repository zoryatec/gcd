using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.PackageBuilder;

public record PackageBuilderDefinition
{
    public LocalDirPath RootDir { get; }
    public LocalDirPath DataDir { get; }
    public LocalDirPath ControlDir { get; }
    //public LocalDirPath ContentDir { get; }
    public LocalFilePath DebianFile { get; }
    public LocalFilePath ControlFile { get; }
    public LocalFilePath InstructionFile { get; }

    public static Result<PackageBuilderDefinition> Of(ILocalDirPath feedDirPath)
    {

        var result =
            from rootDir in LocalDirPath.Of($"{feedDirPath.Value}")
            from dataDir in LocalDirPath.Of($"{feedDirPath.Value}\\data")
            from controlDir in LocalDirPath.Of($"{feedDirPath.Value}\\control")
            from debianFile in LocalFilePath.Of($"{feedDirPath.Value}\\debian-binary")
            from controlFile in LocalFilePath.Of($"{feedDirPath.Value}\\control\\control")
            from instructionsFile in LocalFilePath.Of($"{feedDirPath.Value}\\data\\instructions")
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
