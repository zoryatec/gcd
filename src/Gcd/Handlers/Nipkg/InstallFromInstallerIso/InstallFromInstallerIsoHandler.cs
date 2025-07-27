using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.LocalFileSystem.Abstractions;
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
        
        return await (
                expandDirectory.HasValue
                    ? Task.FromResult(Result.Success(expandDirectory.Value))
                    : fileSystem.GenerateTempDirectoryAsync()
            )
            .Bind(dir => _mediator.ExpandIsoFileAsync(isoFilePath, dir, removeIsoFile, cancellationToken)
                .Map(() => dir))
            .Bind(dir => _mediator.InstallFromInstallerDirectoryAsync(dir, packageMatchPattern, simulateInstallation,
                removeExpandedDirectory, cancellationToken));    
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