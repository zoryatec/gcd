using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Providers.Abstractions;

namespace Gcd.Providers;

public class InstallerDirectoryProvider : IInstallerDirectoryProvider
{
    public Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(IReadOnlyList<LocalDirPath> directories)
    {
        var packagesFilePaths = GetPackageFilePath(directories);

        var packageDefinitions = new List<PackageDefinition>();
        foreach (var path in packagesFilePaths.Value)
        {
            var content = GetPackageFileContent(path);
            var definitions = GetPackageDefinitions(content.Value);
            packageDefinitions.AddRange(definitions.Value);
        }
        return Result.Success<IReadOnlyList<PackageDefinition>>(packageDefinitions); 
    }

    public Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(string content)
    {
        var dictionaries = new InternalOutputParserToBeRefactored().ParsePackagesIntoDictionary(content);
        var packageDefinitions = new List<PackageDefinition>();
        foreach (var package in dictionaries.Value)
        {
            var packageDefinition = new PackageDefinition
            (
                package.ContainsKey("Package") ? package["Package"] : string.Empty,
                package.ContainsKey("Version") ? package["Version"] : string.Empty,
                package.ContainsKey("Description") ? package["Description"] : string.Empty,
                package.ContainsKey("Depends") ? package["Depends"] : string.Empty,
                package.ContainsKey("StoreProduct") ? package["StoreProduct"] : string.Empty,
                package.ContainsKey("UserVisible") ? package["UserVisible"] : string.Empty,
                package.ContainsKey("Section") ? package["Section"] : string.Empty
            );
            packageDefinitions.Add(packageDefinition);
        }
        
        
        return Result.Success<IReadOnlyList<PackageDefinition>>(packageDefinitions);;
    }


    public Result<IReadOnlyList<LocalFilePath>> GetPackageFilePath(IReadOnlyList<LocalDirPath> directories)
    {
        var paths = new List<LocalFilePath>();
        foreach (var directory in directories)
        {
            paths.Add(GetPackageFilePath(directory).Value);
        }

        return Result.Success<IReadOnlyList<LocalFilePath>>(paths);
    }


    public Result<LocalFilePath> GetPackageFilePath(LocalDirPath feedDirPath)=>
        LocalFilePath.Of($"{feedDirPath.Value}\\Packages").MapError(x => x.Message);

    public Result<string> GetPackageFileContent(LocalFilePath packageFilePath)
    {
        string content = File.ReadAllText(packageFilePath.Value);

        return Result.Success(content);;
    }

    public Result<IReadOnlyList<FeedDefinition>> GetFeedDefinitions(IReadOnlyList<LocalDirPath> feedDirectories)
    {
        var feedDefinitions = new List<FeedDefinition>();

        foreach (var directory in feedDirectories)
        {
            var dirName = directory.Value.Split('\\').Last();
            var feedName = "local-feed-" + dirName;
            var feedDefinition = new FeedDefinition(
                Name: feedName,
                Uri: directory.Value
            );
            feedDefinitions.Add(feedDefinition);
        }
        
        return Result.Success<IReadOnlyList<FeedDefinition>>(feedDefinitions.AsReadOnly());
    }

    public Result<IReadOnlyList<LocalDirPath>> GetMainFeedsDirectories(InstallerDirectory installerDirectory)
    {
        var mainFeedDirectories = new List<LocalDirPath>();
        var directories = Directory.GetDirectories(installerDirectory.MainFeedsDirectoryPath.Value);
        foreach (var directory in directories)
        {
            var feedDirectory = LocalDirPath.Of(directory);
            mainFeedDirectories.Add(feedDirectory.Value);
        }
        
        return Result.Success<IReadOnlyList<LocalDirPath>>(mainFeedDirectories.AsReadOnly());
    }
}