using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Shared;

public record DownloadArchiveRequest(
    ILocalDirPath DestinationDirectory,
    WebFileUri ArchiveUri,
    ArchiveFormat ArchiveFormat,
    IRelativeDirPath ArchiveDirectory) : IRequest<UnitResult<Error>>;

public class DownloadArchiveHandler(IWebDownload webDownload)
    : IRequestHandler<DownloadArchiveRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(DownloadArchiveRequest request, CancellationToken cancellationToken)
    {
        var(destinationDir, archiveUri, archiveFormat, archiveDirectory) = request;
        
        // return await DownloadFileAsync(archiveUri,)
        //                 .MapError(x => new Error(x));
        return UnitResult.Failure(new Error($"Invalid destination directory: {destinationDir}"));
    }

    public async Task<Result> DownloadFileAsync(IWebFileUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await webDownload.DownloadFileAsync(webUri, filePath);
    }
}

public record ArchiveFormat
{
    private string _value = string.Empty;

    public Result<ArchiveFormat, Error> Of(Maybe<string> format)
    {
        var frmt = format.Value.ToLower();
        if (frmt.Equals("zip")) return new ArchiveFormat("zip");
        
        return Result.Failure<ArchiveFormat,Error>(new Error($"Unknown format: {frmt}"));
    }

    private ArchiveFormat(string value)
    {
        _value = value;
    }
}


