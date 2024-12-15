using CSharpFunctionalExtensions;
using Gcd.Model;

namespace Gcd.Services;

public interface IFileSystem
{
    public Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default);

    public Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default);

    public Task<Result> CreateDirectoryAsync(LocalDirPath path);

    public Task<Result<bool>> CheckDirectoryExists(LocalDirPath path);

    public Task<Result> CopyFileAsync(LocalFilePath source, LocalFilePath destination, bool overwrite = false, CancellationToken cancellationToken = default);

    public Task<Result<LocalDirPath>> GenerateTempDirectoryAsync();

    public Task<Result<LocalDirPath>> CreateTempDirPathAsync();

}
