using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromInstallerIso;


public record InstallFromInstallerIsoRequest(
    LocalFilePath IsoFilePath, Maybe<LocalDirPath> ExpandDirectory, bool RemoveIsoFile, bool RemoveExpandedDirectory,
    Maybe<string> PackageMatchPattern, bool SimulateInstallation
) : IRequest<Result>;


public class InstallFromInstallerIsoHandler(IMediator _mediator, IFileSystem fileSystem)
    : IRequestHandler<InstallFromInstallerIsoRequest, Result>
{
    public async Task<Result> Handle(InstallFromInstallerIsoRequest request, CancellationToken cancellationToken)
    {
        var (isoFilePath, expandDirectory, removeIsoFile, removeExpandedDirectory
            , packageMatchPattern,simulateInstallation) = request;
        
        if (expandDirectory.HasNoValue)
        {
            var tempDirResult = await fileSystem.GenerateTempDirectoryAsync();
            if (tempDirResult.IsFailure)
                return Result.Failure(tempDirResult.Error);

            expandDirectory = tempDirResult.Value;
        }

        var resultExpand = await _mediator.ExpandIsoFileAsync(isoFilePath, expandDirectory.Value, removeIsoFile, cancellationToken);
        if(resultExpand.IsFailure) {return Result.Failure(resultExpand.Error);}

        var result = await _mediator.InstallFromInstallerDirectoryAsync(expandDirectory.Value,packageMatchPattern,simulateInstallation,
            cancellationToken);
        return result;
    }
}

public static class MediatorIsoExtensions
{
    public static async Task<Result> InstallFromInstallerIsoAsync(
        this IMediator mediator,
        LocalFilePath isoFilePath, Maybe<LocalDirPath> expandDirectory, bool removeIsoFile, bool removeExpandedDirectory,
        Maybe<string> packageMatchPattern, bool simulateInstallation,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromInstallerIsoRequest(isoFilePath, expandDirectory,removeIsoFile,
            removeExpandedDirectory,packageMatchPattern, simulateInstallation), cancellationToken);
}