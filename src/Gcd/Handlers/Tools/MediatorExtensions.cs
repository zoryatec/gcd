using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Model.File;
using MediatR;

namespace Gcd.Handlers.Tools;
public static class MediatorExtensions
{
    public static async Task<Result> DownloadNipkgInstallerAsync(this IMediator mediator, LocalFilePath FilePath, NipkgInstallerUri InstallerUri, CancellationToken cancellationToken = default)
        => await mediator.Send(new DownloadNipkgRequest(FilePath, InstallerUri), cancellationToken)
           .MapError(x => x.Message);   

    public static async Task<Result> InstallNipkgInstallerAsync(this IMediator mediator, NipkgCmdPath cmdPath, CancellationToken cancellationToken = default)
    => await mediator.Send(new InstallNinpkgRequest(cmdPath), cancellationToken);


    public static async Task<Result> AddToUserPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
    => await mediator.Send(new AddToPathRequest(pathToAdd, EnvironmentVariableTarget.User), cancellationToken);
    public static async Task<Result> AddToSystemPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddToPathRequest(pathToAdd, EnvironmentVariableTarget.Machine), cancellationToken);
}
