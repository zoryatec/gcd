using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Tools;

public record BootstrapRequest(NipkgInstallerUri InstallerUri, string GcdFeed, string GcdPackageName, string GcdPackageVersion) : IRequest<Result>;


public class BootstrapHandler(IMediator mediator, INiPackageManagerExtendedService nipm)
    : IRequestHandler<BootstrapRequest, Result>
{
    private const string NipmConainingDir = @"C:\Program Files\National Instruments\NI Package Manager";
    private const string GcdContainingDir = @"C:\Program Files\gcd";
    public async Task<Result> Handle(BootstrapRequest request, CancellationToken cancellationToken)
    {
        var feed = new FeedDefinition("gcd-feed",request.GcdFeed);
        var package = new PackageDefinition(request.GcdPackageName, request.GcdPackageVersion);

        return await mediator.Send(new InstallNinpkgRequest(request.InstallerUri, NipkgCmdPath.Deafault),
                cancellationToken)
            .Bind(() => nipm.InstallFeedAsync(feed))
            .Bind(() => nipm.InstallPackageAsync(package, 
                true, true,false, true, 
                false, true, true, 
                true, true))
            .Bind(() => mediator.AddToPathAsync(NipmConainingDir, cancellationToken))
            .Bind(() => mediator.AddToPathAsync(GcdContainingDir, cancellationToken));
    }
}

