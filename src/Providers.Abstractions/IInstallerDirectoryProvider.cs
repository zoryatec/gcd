using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;

namespace Providers.Abstractions;

public record InstallerDirectory(LocalDirPath InstallerDirectoryPath)
{

    public LocalDirPath MainFeedsDirectoryPath => LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\feeds").Value;
    public LocalDirPath NiPackageManagerFeedsDirectoryPath = LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\bin\\ni-package-manager-packages").Value;
}


public interface IInstallerDirectoryProvider
{
    Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(IReadOnlyList<LocalDirPath> directories);
    Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(string content);
    Result<IReadOnlyList<LocalFilePath>> GetPackageFilePath(IReadOnlyList<LocalDirPath> directories);
    Result<LocalFilePath> GetPackageFilePath(LocalDirPath feedDirPath);
    Result<string> GetPackageFileContent(LocalFilePath packageFilePath);
    Result<IReadOnlyList<FeedDefinition>> GetFeedDefinitions(IReadOnlyList<LocalDirPath> feedDirectories);
    Result<IReadOnlyList<LocalDirPath>> GetMainFeedsDirectories(InstallerDirectory installerDirectory);
}