using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.Nipkg.Builder;

public record PackRequest(BuilderRootDir RootDir, PackageDestinationDirectory OutputDir, NipkgCmdPath CmdPath) : IRequest<Result>;

public class PackHandler(IMediator _mediator)
    : IRequestHandler<PackRequest, Result>
{
    public async Task<Result> Handle(PackRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, outputDir, cmd) = request;
        return await _mediator.NipkgPackAsync(rootDir, outputDir, cmd);
    }
}

public static class MediatorExtensionsPack
{
    public static async Task<Result> BuilderPackAsync(this IMediator mediator, BuilderRootDir rootDir, PackageDestinationDirectory outputDir, NipkgCmdPath cmd, CancellationToken cancellationToken = default)
        => await mediator.Send(new PackRequest(rootDir, outputDir, cmd), cancellationToken);
}