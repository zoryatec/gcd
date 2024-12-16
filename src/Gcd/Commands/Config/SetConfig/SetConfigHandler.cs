using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;


//https://www.ni.com/docs/en-US/bundle/package-manager/page/installation-target-roots.html


using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;



namespace Gcd.Commands.Config.SetConfig;

public record SetConfigRequest(PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks) : IRequest<Result>;

public class SetConfigHandler(IFileSystem _fs)
    : IRequestHandler<SetConfigRequest, Result>
{
    public async Task<Result> Handle(SetConfigRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, contentLinks) = request;
        var results = new List<Result>();


        return Result.Combine(results);
    }





}

public static class MediatorExtensions
{
    public static async Task<Result> SetConfigAsync(this IMediator mediator, PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks, CancellationToken cancellationToken = default)
        => await mediator.Send(new SetConfigRequest(rootDir, contentLinks), cancellationToken);
    public static async Task<Result> SetConfigAsync(this IMediator mediator, PackageBuilderRootDir rootDir, ContentLink contentLink, CancellationToken cancellationToken = default)
    => await mediator.Send(new SetConfigRequest(rootDir, new List<ContentLink> { contentLink }), cancellationToken);

    public static async Task<Result> SetConfigAsync(this IMediator mediator, PackageBuilderRootDir rootDir, InatallationTargetRootDir targetRootDir, PackageBuilderContentSourceDir contentSourceDir, CancellationToken cancellationToken = default)
=> await mediator.Send(new SetConfigRequest(rootDir, new List<ContentLink> { new ContentLink(targetRootDir, contentSourceDir) }), cancellationToken);
}