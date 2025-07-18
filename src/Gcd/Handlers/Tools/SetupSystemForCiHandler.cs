

using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Tools;

public record SetupSystemForCiRequest(LocalFilePath LabbViewIniFilePath, LocalFilePath LabViewCliIniFilePath) : IRequest<UnitResult<Error>>;

public class SetupSystemForCiHandler(IMediator mediator)
    : IRequestHandler<SetupSystemForCiRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetupSystemForCiRequest request, CancellationToken cancellationToken)
    {
        var(labViewIniFilePath, labViewCliFilePath) = request;
        
        return await mediator.SetIniFileParameteAsync(labViewIniFilePath, "LabVIEW", "server.tcp.enabled", "True",true,cancellationToken)
            .Bind(() => mediator.SetIniFileParameteAsync(labViewIniFilePath, "LabVIEW", "server.tcp.port", "3363",true,cancellationToken))
            .Bind(() => mediator.SetIniFileParameteAsync(labViewIniFilePath, "LabVIEW", "neverShowAddonLicensingStartup", "True",true,cancellationToken))
            .Bind(() => mediator.SetIniFileParameteAsync(labViewIniFilePath, "LabVIEW", "neverShowLicensingStartupDialog", "True",true,cancellationToken))
            .Bind(() => mediator.SetIniFileParameteAsync(labViewIniFilePath, "LabVIEW", "ShowWelcomeOnLaunch", "False",true,cancellationToken))
            .Bind(() => mediator.SetIniFileParameteAsync(labViewCliFilePath, "LabVIEWCLI", "ValidateLabVIEWLicense", "FALSE",true,cancellationToken));
    }
}