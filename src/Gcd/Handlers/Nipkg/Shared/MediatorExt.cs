using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Handlers.Nipkg.Replication;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using MediatR;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;

namespace Gcd.Handlers.Nipkg.Shared;

public static class MediatorExt
{
    public static async Task<Result> UploadPackageAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, PackageLocalFilePath packageFilePath, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new UploadPackageRequest<TFeedDefinition>(remoteFeedDef, new List<PackageLocalFilePath>{packageFilePath}.AsReadOnly()), cancellationToken);
    
    public static async Task<Result> UploadPackageAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, IReadOnlyList<PackageLocalFilePath> packageFilePaths, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new UploadPackageRequest<TFeedDefinition>(remoteFeedDef, packageFilePaths), cancellationToken);

    public static async Task<Result> PushFeedMetaDataAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new PushFeedMetaRequest<TFeedDefinition>(FeedDefinition, LocalFeedDef), cancellationToken);

    public static async Task<Result> PullFeedMetaAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition remoteFeedDef, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
    => await mediator.Send(new PullFeedMetaRequest<TFeedDefinition>(remoteFeedDef, LocalFeedDef), cancellationToken);
    
    public static async Task<Result> GetSystemPackages(this IMediator mediator, CancellationToken cancellationToken = default) 
        => await mediator.Send(new GetSystemPackagesRequest(), cancellationToken);

    public static async Task<Result> AddPackageToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator,
    TFeedDefinition remoteFeedDef,
    IPackageFileDescriptor PackagePath,
    NipkgCmdPath cmdPath,
    UseAbsolutePath useAbsolutePath,
    bool createFeed = false,
    CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
    => await mediator.Send(new AddPackageToRemoteFeedRequest<TFeedDefinition>(
        remoteFeedDef, 
        [PackagePath], 
        cmdPath,
        useAbsolutePath,
        createFeed), cancellationToken);
    
    public static async Task<Result> AddPackageToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator,
        TFeedDefinition remoteFeedDef,
        IReadOnlyList<IPackageFileDescriptor> packagePaths,
        NipkgCmdPath cmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new AddPackageToRemoteFeedRequest<TFeedDefinition>(remoteFeedDef, packagePaths, cmdPath, useAbsolutePath, createFeed), cancellationToken);
    
    public static async Task<Result> AddDirToRemoteFeedAsync<TFeedDefinition>(this IMediator mediator,
        TFeedDefinition remoteFeedDef,
        ILocalDirPath packageSourceDirectory,
        NipkgCmdPath cmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new AddDirectoryToRemoteFeedRequest<TFeedDefinition>(remoteFeedDef, packageSourceDirectory, cmdPath, useAbsolutePath, createFeed), cancellationToken);
}

