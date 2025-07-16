using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using MediatR;
using Snapshot.Abstractions;

namespace Gcd.Handlers.Nipkg.Snapshot;


public record CreateSnapshotFromInstallerResponse(global::Snapshot.Abstractions.Snapshot Snapshot);
public record CreateSnapshotFromInstallerRequest(
    LocalDirPath InstallerDirectory
) : IRequest<Result<CreateSnapshotFromInstallerResponse>>;


public class CreateFromInstallerDirectoryHandler(IMediator _mediator)
    : IRequestHandler<CreateSnapshotFromInstallerRequest, Result<CreateSnapshotFromInstallerResponse>>
{
    public async Task<Result<CreateSnapshotFromInstallerResponse>> Handle(CreateSnapshotFromInstallerRequest request, CancellationToken cancellationToken)
    {
        var installerDirectoryPath = request.InstallerDirectory;
        var installerDir = new InstallerDirectory(installerDirectoryPath);
        var directories = GetMainFeedsDirectories(installerDir);;
        var feedDefinitions = GetFeedDefinitions(directories.Value);
        var packageDefinitions = GetPackageDefinitions(directories.Value);
        
        var snapshot = new global::Snapshot.Abstractions.Snapshot(packageDefinitions.Value, feedDefinitions.Value);
        
        return Result.Success(new CreateSnapshotFromInstallerResponse(snapshot));
    }

    private Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(IReadOnlyList<LocalDirPath> directories)
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
    
    private Result<IReadOnlyList<PackageDefinition>> GetPackageDefinitions(string content)
    {
        var dictionaries = new OutputParser().ParsePackagesIntoDictionary(content);
        
        var packageDefinitions = new List<PackageDefinition>();
        foreach (var package in dictionaries.Value)
        {
            var packageDefinition = new PackageDefinition
            (
                package.ContainsKey("Package") ? package["Package"] : string.Empty,
                package.ContainsKey("Version") ? package["Version"] : string.Empty,
                package.ContainsKey("Description") ? package["Description"] : string.Empty,
                package.ContainsKey("Depends") ? package["Depends"] : string.Empty
            );
            packageDefinitions.Add(packageDefinition);
        }
        
        
        return Result.Success<IReadOnlyList<PackageDefinition>>(packageDefinitions);;
    }
    
    
    private Result<IReadOnlyList<LocalFilePath>> GetPackageFilePath(IReadOnlyList<LocalDirPath> directories)
    {
        var paths = new List<LocalFilePath>();
        foreach (var directory in directories)
        {
            paths.Add(GetPackageFilePath(directory).Value);
        }

        return Result.Success<IReadOnlyList<LocalFilePath>>(paths);
    }
    
    
    private Result<LocalFilePath> GetPackageFilePath(LocalDirPath feedDirPath)=>
        LocalFilePath.Of($"{feedDirPath.Value}\\Packages").MapError(x => x.Message);

    private Result<string> GetPackageFileContent(LocalFilePath packageFilePath)
    {
        string content = File.ReadAllText(packageFilePath.Value);

        return Result.Success(content);;
    }
    
    private Result<IReadOnlyList<FeedDefinition>> GetFeedDefinitions(IReadOnlyList<LocalDirPath> feedDirectories)
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

    private Result<IReadOnlyList<LocalDirPath>> GetMainFeedsDirectories(InstallerDirectory installerDirectory)
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

public static class MediatorExtensions
{
    public static async Task<Result<CreateSnapshotFromInstallerResponse>> CreateSnapshotFromInstallerAsync(
        this IMediator mediator,
        LocalDirPath snapshotFilePath,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new CreateSnapshotFromInstallerRequest(snapshotFilePath), cancellationToken);
}


public record InstallerDirectory(LocalDirPath InstallerDirectoryPath)
{

    public LocalDirPath MainFeedsDirectoryPath => LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\feeds").Value;
    public LocalDirPath NiPackageManagerFeedsDirectoryPath = LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\bin\\ni-package-manager-packages").Value;
}