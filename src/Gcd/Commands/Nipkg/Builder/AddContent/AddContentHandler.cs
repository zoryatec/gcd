﻿using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;


//https://www.ni.com/docs/en-US/bundle/package-manager/page/installation-target-roots.html


using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;



namespace Gcd.Commands.Nipkg.Builder.AddContent;

public record AddContentRequest(PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks) : IRequest<Result>;

public class AddContentHandler(IFileSystem _fs)
    : IRequestHandler<AddContentRequest, Result>
{
    public async Task<Result> Handle(AddContentRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, contentLinks) = request;
        var results = new List<Result>();

        contentLinks.ForEach(async link => results.Add(await AddContent(rootDir, link)));
        return Result.Combine(results);
    }

    public async Task<Result> AddContent(PackageBuilderRootDir rootDir, ContentLink contentLink) =>
        await PackageBuilderContentDir.Of(rootDir, contentLink.TargetRootDir)
            .Bind(dir => _fs.CopyDirectoryRecursievely(contentLink.ContentSourceDir, dir));



}

public static class MediatorExtensions
{
    public static async Task<Result> AddContentAsync(this IMediator mediator, PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddContentRequest(rootDir, contentLinks), cancellationToken);
    public static async Task<Result> AddContentAsync(this IMediator mediator, PackageBuilderRootDir rootDir, ContentLink contentLink, CancellationToken cancellationToken = default)
    => await mediator.Send(new AddContentRequest(rootDir, new List<ContentLink> { contentLink }), cancellationToken);

    public static async Task<Result> AddContentAsync(this IMediator mediator, PackageBuilderRootDir rootDir, InatallationTargetRootDir targetRootDir, PackageBuilderContentSourceDir contentSourceDir, CancellationToken cancellationToken = default)
=> await mediator.Send(new AddContentRequest(rootDir, new List<ContentLink> { new ContentLink(targetRootDir, contentSourceDir) }), cancellationToken);
}