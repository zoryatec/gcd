using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Services;

public record WebUri
{
    public static Result<WebUri> Create(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult($"{nameof(WebUri)} should not be empty")
        .Ensure(arg => arg != string.Empty, "{nameof(WebUri)}  should not be empty")
        .MapTry((arg) => new Uri(arg), ex => ex.Message)
        .Map(arg => new WebUri(arg));
    }
    protected WebUri(Uri value) => _uri = value;
    private Uri _uri;
    public string Value { get => _uri.AbsoluteUri; }
}

public interface IWebDownload
{
    public Task<Result> DownloadFileAsync(WebUri blobUri, ILocalFilePath filePath);
}