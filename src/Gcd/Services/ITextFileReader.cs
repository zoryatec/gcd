using CSharpFunctionalExtensions;
using Gcd.Model;

namespace Gcd.Services;

public interface ITextFileReader
{
    public Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default);
}

