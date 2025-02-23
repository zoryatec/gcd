using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Shared;

public record DownloadFileRequest(LocalFilePath FilePath, IWebFileUri Uri) : IRequest<UnitResult<Error>>;

public class DownloadFileHandler(IWebDownload webDownload)
    : IRequestHandler<DownloadFileRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(DownloadFileRequest request, CancellationToken cancellationToken)
    {
        var(filePath, uri) = request;
        
        return await DownloadFileAsync(uri,filePath)
                        .MapError(x => new Error(x));
    }

    private async Task<Result> DownloadFileAsync(IWebFileUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await webDownload.DownloadFileAsync(webUri, filePath);
    }
}

public static class DownloadFileHandlerExtensions
{
    public static async Task<UnitResult<Error>> DownloadFileAsync(
        this IMediator mediator,
        LocalFilePath filePath,
        WebFileUri webFileUri) =>
        await mediator.Send(new DownloadFileRequest(filePath, webFileUri));
}


