using CSharpFunctionalExtensions;

namespace Gcd.Services;

public interface ITextFileWriter
{
    public Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default);

}
