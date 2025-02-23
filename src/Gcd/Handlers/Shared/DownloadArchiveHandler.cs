using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Shared;

public record DownloadArchiveRequest(
    WebFileUri ArchiveUri,
    IRelativeDirPath RelativeContentDirPath,
    ILocalDirPath DestinationDirPath
    ) : IRequest<UnitResult<Error>>;

public class DownloadArchiveHandler(IMediator mediator, IFileSystem fileSystem )
    : IRequestHandler<DownloadArchiveRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(DownloadArchiveRequest request, CancellationToken cancellationToken)
    {
        var(archiveUri, relativeContentDirPath,destinationDir) = request;

        var fileName = archiveUri.FileName;
        // var tmpDirectory = ;
        var tempDir = await fileSystem.GenerateTempDirectoryAsync();
        var tempFilePath = new LocalFilePath(tempDir.Value,fileName);
        var resultDownload  = await mediator.DownloadFileAsync(tempFilePath, archiveUri,cancellationToken);
        var resultExtract = await mediator.ExtractArchiveAsync(tempFilePath,relativeContentDirPath,destinationDir,cancellationToken);

        return resultExtract;
    }
}

public static class DownloadArchiveHandlerMediatorExtensions
{
    public static async Task<UnitResult<Error>> DownloadArchiveAsync(
        this IMediator mediator,
        WebFileUri archiveUri,
        IRelativeDirPath relativeContentDirPath,
        ILocalDirPath destinationDirPath,
        CancellationToken cancellationToken
    ) =>
        await mediator.Send(new DownloadArchiveRequest(archiveUri, relativeContentDirPath,destinationDirPath),cancellationToken);
}

