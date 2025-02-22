using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Shared;

public record DownloadHttpFileRequest(LocalFilePath FilePath, IWebFileUri Uri) : IRequest<UnitResult<Error>>;

public class DownloadHttpFileHandler(IWebDownload webDownload)
    : IRequestHandler<DownloadHttpFileRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(DownloadHttpFileRequest request, CancellationToken cancellationToken)
    {
        var(filePath, uri) = request;
        
        return await DownloadFileAsync(uri,filePath)
                        .MapError(x => new Error(x));
    }

    public async Task<Result> DownloadFileAsync(IWebFileUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await webDownload.DownloadFileAsync(webUri, filePath);
    }
}


