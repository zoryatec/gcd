using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Model.Config;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;

public static class MediatorExt
{
    public static async Task<Result> UploadPackageAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, PackageFilePath packageFilePath, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new UploadPackageRequest<TFeedDefinition>(remoteFeedDef, packageFilePath), cancellationToken);

    public static async Task<Result> PushFeedMetaDataAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new PushFeedMetaRequest<TFeedDefinition>(FeedDefinition, LocalFeedDef), cancellationToken);

    public static async Task<Result> PullFeedMetaAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
    => await mediator.Send(new PullFeedMetaRequest<TFeedDefinition>(remoteFeedDef, LocalFeedDef), cancellationToken);

    public static async Task<Result> AddPackageToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator,
    TFeedDefinition remoteFeedDef,
    IPackageFileDescriptor PackagePath,
    NipkgCmdPath cmdPath,
    UseAbsolutePath useAbsolutePath,
    bool createFeed = false,
    CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
    => await mediator.Send(new AddPackageToRemoteFeedRequest<TFeedDefinition>(remoteFeedDef, PackagePath, cmdPath, useAbsolutePath, createFeed), cancellationToken);
}

