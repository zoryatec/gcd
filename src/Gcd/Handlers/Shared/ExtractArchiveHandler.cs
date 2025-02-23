using System.IO.Compression;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using MediatR;

namespace Gcd.Handlers.Shared;

public record ExtractArchiveRequest(
    ILocalFilePath FilePath,
    IRelativeDirPath RelativeContentDirPath,
    ILocalDirPath DestinationDirPath) 
    : IRequest<UnitResult<Error>>;

public class ExtractArchiveHandler(IFileSystem FileSystem)
    : IRequestHandler<ExtractArchiveRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(ExtractArchiveRequest request, CancellationToken cancellationToken)
    {
        var(archiveFilePath, contentDirectory, destinationDirPath) = request;
        var tempDirectoryToExtractResult = await FileSystem.GenerateTempDirectoryAsync();
        var tempDirectoryToExtract = tempDirectoryToExtractResult.Value;
        if(!archiveFilePath.FileName.Extension.Equals(FileExtension.Zip))
            return UnitResult.Failure<Error>(new Error("not supported archive file extension"));
        
        var dirToMoveContentFrom = LocalDirPath.Of(tempDirectoryToExtract, contentDirectory);
        ZipFile.ExtractToDirectory(archiveFilePath.Value, tempDirectoryToExtract.Value);
        var copyResult = await FileSystem.CopyDirectoryRecursievely(dirToMoveContentFrom.Value, destinationDirPath, overwrite: true,  cancellationToken);

        if (!copyResult.IsSuccess) return UnitResult.Success<Error>();
        return UnitResult.Failure<Error>(new Error($"Some Error."));
    }
    
}

public static class ExtractArchiveRequestMediatorExtensions
{
    public static async Task<UnitResult<Error>> ExtractArchiveAsync(
        this IMediator mediator,
        ILocalFilePath filePath,
        IRelativeDirPath relativeContentDirPath,
        ILocalDirPath destinationDirPath,
        CancellationToken cancellationToken
        ) =>
        await mediator.Send(new ExtractArchiveRequest(filePath, relativeContentDirPath, destinationDirPath), cancellationToken);
}


