using System.Text;
using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LocalFileSystem.Abstractions;

public interface IFileSystem
{
    public Result<IReadOnlyList<ILocalFilePath>, Error> ListFiles(ILocalDirPath directoryPath,
        string searchPattern = "*.*", bool recursive = false);
    public Task<Result> WriteTextFileAsync(ILocalFilePath filePath, string content, CancellationToken cancellationToken = default);
    public Task<Result> WriteAllLinesAsync(ILocalFilePath filePath, IEnumerable<string> content, Encoding encoding, CancellationToken cancellationToken = default);

    public Task<Result<string>> ReadTextFileAsync(ILocalFilePath filePath, CancellationToken cancellationToken = default);
    
    public Result<bool> FileExists(ILocalFilePath filePath);
    public Result<bool> DirectoryExists(ILocalDirPath dirPath);

    public Task<Result> CreateDirectoryAsync(LocalDirPath path);

    public Task<Result<bool>> CheckDirectoryExists(LocalDirPath path);

    public Task<Result> CopyFileAsync(ILocalFilePath source, ILocalFilePath destination, bool overwrite = false, CancellationToken cancellationToken = default);

    public Task<Result<LocalDirPath>> GenerateTempDirectoryAsync();

    public Task<Result<LocalDirPath>> CreateTempDirPathAsync();

    public Task<Result> CopyDirectoryRecursievely(ILocalDirPath source, ILocalDirPath destination, bool overwrite = false, CancellationToken cancellationToken = default);

    public Task<Result> CreateDirAsync(LocalDirPath locDirPath);

}
