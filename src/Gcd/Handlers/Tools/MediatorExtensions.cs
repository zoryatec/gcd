using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using MediatR;

namespace Gcd.Handlers.Tools;
public static class MediatorExtensions
{
    public static async Task<Result> DownloadNipkgInstallerAsync(this IMediator mediator, LocalFilePath FilePath, NipkgInstallerUri InstallerUri, CancellationToken cancellationToken = default)
        => await mediator.Send(new DownloadNipkgRequest(FilePath, InstallerUri), cancellationToken)
           .MapError(x => x.Message);   

    public static async Task<Result> InstallNipkgInstallerAsync(this IMediator mediator, NipkgInstallerUri installerUri, NipkgCmdPath cmdPath, CancellationToken cancellationToken = default)
    => await mediator.Send(new InstallNinpkgRequest(installerUri,cmdPath), cancellationToken);
    
    public static async Task<Result> AddToUserPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
    => await mediator.Send(new AddToPathRequest(pathToAdd, EnvironmentVariableTarget.User), cancellationToken);
    public static async Task<Result> AddToSystemPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddToPathRequest(pathToAdd, EnvironmentVariableTarget.Machine), cancellationToken);
    public static async Task<UnitResult<Error>> KillProcessAsync(this IMediator mediator,ProcessName processName, CancellationToken cancellationToken = default)
        => await mediator.Send(new KillProcessRequest(processName), cancellationToken);
    public static async Task<UnitResult<Error>> SetIniFileParameteAsync(this IMediator mediator, LocalFilePath iniFilePath,
        string section, string key, string value, bool createIfNotExists = true, 
        CancellationToken cancellationToken = default)
        => await mediator.Send(new SetInifFileParameterRequest(iniFilePath,section,key,value,createIfNotExists), cancellationToken);
    public static async Task<UnitResult<Error>> SetupSystemForCi(this IMediator mediator, LocalFilePath labViewIniFilePath,
        LocalFilePath labViewCliFilePath, CancellationToken cancellationToken = default)
        => await mediator.Send(new SetupSystemForCiRequest(labViewIniFilePath,labViewCliFilePath), cancellationToken);
}
