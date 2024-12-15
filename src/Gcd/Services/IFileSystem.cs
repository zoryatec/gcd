using CSharpFunctionalExtensions;
using Gcd.Model;

namespace Gcd.Services;

public interface IFileSystem
{
    public Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default);

    public Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default);

}
