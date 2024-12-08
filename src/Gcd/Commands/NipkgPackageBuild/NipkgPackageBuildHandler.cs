using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgAddPackageToAzFeed;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Handlers;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuild;

public record PackageContentDir
{
    public static Result<PackageContentDir> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageContentDir)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageContentDir)} should not be empty")
            .Map(feedUri => new PackageContentDir(feedUri));
    
    private PackageContentDir(string path) => Value = path;
    public string Value { get; }
}

public record PackageName
{
    public static Result<PackageName> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageName)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageName)} should not be empty")
            .Map(feedUri => new PackageName(feedUri));

    private PackageName(string path) => Value = path;
    public string Value { get; }
}

public record PackageVersion
{
  public static Result<PackageVersion> Create(Maybe<string> packagePathOrNothing) =>
         packagePathOrNothing.ToResult($"{nameof(PackageVersion)} should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageVersion)} should not be empty")
            .Map(feedUri => new PackageVersion(feedUri));

    private PackageVersion(string path) => Value = path;
    public string Value { get; }
}

public record PackageInstalationDir
{
    public static Result<PackageInstalationDir> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(PackageInstalationDir)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageInstalationDir)} should not be empty")
              .Map(feedUri => new PackageInstalationDir(feedUri));

    private PackageInstalationDir(string path) => Value = path;
    public string Value { get; }
}

public record PackageDestinationDirectory
{
    public static Result<PackageDestinationDirectory> Create(Maybe<string> packagePathOrNothing) =>
           packagePathOrNothing.ToResult($"{nameof(PackageDestinationDirectory)} should not be empty")
              .Ensure(packagePath => packagePath != string.Empty, $"{nameof(PackageDestinationDirectory)} should not be empty")
              .Map(feedUri => new PackageDestinationDirectory(feedUri));

    private PackageDestinationDirectory(string path) => Value = path;
    public string Value { get; }
}


public record PackageBuildRequest(PackageContentDir PackageContentPath, PackageName PackageName, PackageVersion PackageVersion, PackageInstalationDir PackageInstalationDir, PackageDestinationDirectory PackageDestinationDir) : IRequest<Result>;


public class PackageBuildHandler(IMediator _mediator)
    : IRequestHandler<PackageBuildRequest, Result>
{
    public async Task<Result> Handle(PackageBuildRequest request, CancellationToken cancellationToken)
    {

        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string pckgDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());


        if (!Directory.Exists(pckgDirectory))
        {
            // Delete the directory
            Directory.CreateDirectory(pckgDirectory);
        }


        var temporaryDir = PackageContentDir.Create(temporaryDirectory);
        var subRequest = new PackageBuilderInitRequest(temporaryDir.Value, request.PackageName, request.PackageVersion, request.PackageInstalationDir);
        var subResponse = await _mediator.Send(subRequest);

        var contnetDestinationPaht = $"{temporaryDirectory}\\data\\{request.PackageInstalationDir.Value}";
        CopyDirectoryContents(request.PackageContentPath.Value, contnetDestinationPaht);

        var result = RunCommand(temporaryDirectory, pckgDirectory);
        string packageFileName = $"{request.PackageName.Value}_{request.PackageVersion.Value}_windows_x64.nipkg";
        string packageFilePath = Path.Combine(pckgDirectory, packageFileName);

        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDestinationDir = Path.Combine(currentDirectoryPath, request.PackageDestinationDir.Value);

        if (!Directory.Exists(packageDestinationDir))
        {
            // Delete the directory
            Directory.CreateDirectory(packageDestinationDir);
        }


        string packageDestinationFilePath = Path.Combine(packageDestinationDir, packageFileName);
        File.Copy(packageFilePath, packageDestinationFilePath, overwrite: true);

        Directory.Delete(temporaryDirectory, true);

        Directory.Delete(pckgDirectory, true);

        return Result.Success();
    }

    static void CopyDirectoryContents(string sourceDir, string destinationDir)
    {
        // Ensure the source directory exists
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDir}");
        }

        // Create the destination directory if it does not exist
        Directory.CreateDirectory(destinationDir);

        // Copy all files from source to destination
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destinationDir, fileName);
            File.Copy(file, destFile, overwrite: true);
        }

        // Recursively copy all subdirectories
        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            string directoryName = Path.GetFileName(directory);
            string destDir = Path.Combine(destinationDir, directoryName);
            CopyDirectoryContents(directory, destDir);
        }
    }

    private async Task<Result> RunCommand(string temporaryDirectory, string pckgDirectory)
    {
        var arguments = new string[] { "pack", temporaryDirectory, pckgDirectory };
        var req = new RunNipkgRequest(arguments);
        return await _mediator.Send(req);
    }
}

