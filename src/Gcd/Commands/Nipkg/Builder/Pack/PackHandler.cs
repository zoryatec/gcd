using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.Nipkg.Builder.Pack;

public record PackRequest(PackageBuilderRootDir RootDir, PackageDestinationDirectory OutputDir) : IRequest<Result>;

public class PackHandler(IMediator _mediator)
    : IRequestHandler<PackRequest, Result>
{
    public async Task<Result> Handle(PackRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, outputDir) = request;
        return await _mediator.NipkgPackAsync(rootDir, outputDir);
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> BuilderPackAsync(this IMediator mediator, PackageBuilderRootDir rootDir, PackageDestinationDirectory outputDir, CancellationToken cancellationToken = default)
        => await mediator.Send(new PackRequest(rootDir, outputDir), cancellationToken);
}