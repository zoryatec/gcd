
using CSharpFunctionalExtensions;
using DiscUtils.Iso9660;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromInstallerIso;


public record InstallFromInstallerDirectoryResponse();
public record ExpandIsoFileRequest(
    LocalFilePath IsoFilePath, LocalDirPath ExpandDirectory, bool RemoveIsoFile
) : IRequest<Result>;


public class ExpandIsoFileHandler(IMediator _mediator)
    : IRequestHandler<ExpandIsoFileRequest, Result>
{
    public async Task<Result> Handle(ExpandIsoFileRequest request, CancellationToken cancellationToken)
    {
        await using (var isoStream = File.OpenRead(request.IsoFilePath.Value))
        {
            var cd = new DiscUtils.Iso9660.CDReader(isoStream, true, false); // useJoliet: true
            foreach (var dirPath in cd.GetDirectories("\\", "*", SearchOption.AllDirectories).Prepend("\\"))
            {
                var relativeDir = dirPath.TrimStart('\\');
                var outDir = Path.Combine(request.ExpandDirectory.Value, relativeDir);
                Directory.CreateDirectory(outDir);

                foreach (var filePath in cd.GetFiles(dirPath))
                {
                    var relativePath = filePath.TrimStart('\\');
                    var outPath = Path.Combine(request.ExpandDirectory.Value, relativePath);

                    await using var fileStream = cd.OpenFile(filePath, FileMode.Open);
                    await using var outFile = File.Create(outPath);
                    await fileStream.CopyToAsync(outFile, cancellationToken);
                }
            }
        }
        return Result.Success();
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> ExpandIsoFileAsync(
        this IMediator mediator,
        LocalFilePath isoFilePath,
        LocalDirPath expandDirectory,
        bool removeIsoFile,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new ExpandIsoFileRequest(isoFilePath, expandDirectory,removeIsoFile), cancellationToken);
}