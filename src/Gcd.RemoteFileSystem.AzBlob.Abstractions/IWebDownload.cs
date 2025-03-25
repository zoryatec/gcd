using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Services;

public record WebUri : IWebUri
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

public interface IWebUri
{
    public string Value { get; }
}

public interface IWebFileUri
{
    public string Value { get; }
    public FileName FileName { get; }
}

public record WebFileUri : IWebFileUri
{
    public static Result<WebFileUri> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult($"{nameof(WebFileUri)} should not be empty")
            .Ensure(arg => arg != string.Empty, "{nameof(WebUri)}  should not be empty")
            .MapTry((arg) => new Uri(arg), ex => ex.Message)
            .Ensure(uri => FileName.Of(uri.Segments.Last()).IsSuccess, $"{nameof(WebFileUri)}  should  be file")
            .Map(arg => new WebFileUri(arg));
    }

    protected WebFileUri(Uri value)
    {
        _uri = value;
        var fileName = _uri.Segments.Last().Trim('/');
        FileName = FileName.Of(fileName).Value; // checked on static factory

    }
    private Uri _uri;
    public string Value { get => _uri.AbsoluteUri; }
    public FileName FileName { get ; }
}

public interface IWebDownload
{
    public Task<Result> DownloadFileAsync(IWebFileUri webFileUri, ILocalFilePath filePath);
    public Task<Result> DownloadFileAsync(IWebFileUri webFileUri, ILocalDirPath directoryPath);
}