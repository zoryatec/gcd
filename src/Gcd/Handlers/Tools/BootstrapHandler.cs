using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Tools;

public record BootstrapRequest(NipkgInstallerUri InstallerUri) : IRequest<Result>;


public class BootstrapHandler(IMediator mediator, INiPackageManagerExtendedService nipm)
    : IRequestHandler<BootstrapRequest, Result>
{
    private const string GcdFeed = "https://zoryatecartifacts.blob.core.windows.net/dev-feed";
    private const string NipmConainingDir = @"C:\Program Files\National Instruments\NI Package Manager";
    private const string GcdContainingDir = @"C:\Program Files\gcd";
    public async Task<Result> Handle(BootstrapRequest request, CancellationToken cancellationToken)
    {
        var feed = new FeedDefinition("gcd-dev-feed",GcdFeed);
        var package = new PackageDefinition("gcd-dev", "");

        return await mediator.Send(new InstallNinpkgRequest(request.InstallerUri, NipkgCmdPath.Deafault),
                cancellationToken)
            .Bind(() => nipm.InstallFeedAsync(feed))
            .Bind(() => nipm.InstallPackageAsync(package, false))
            .Bind(() => mediator.AddToPathAsync(NipmConainingDir, cancellationToken))
            .Bind(() => mediator.AddToPathAsync(GcdContainingDir, cancellationToken));
    }
}

