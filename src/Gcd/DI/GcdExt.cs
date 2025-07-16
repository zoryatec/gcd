using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LabViewProject;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.RemoteFileSystem.AzBlob.DI;
using Gcd.RemoteFileSystem.Git.DI;
using Gcd.RemoteFileSystem.Smb.DI;
using Gcd.Services;
using Gcd.Services.DI;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using Gcd.RemoteFileSystem.Rclone.DI;
using Gcd.Snapshot;
using Gcd.SystemProcess;
using Gcd.SystemProcess.Abstractions;
using Snapshot.Abstractions;

namespace Gcd.DI;

public static class GcdExt
{

    public static IServiceCollection AddGcd(this IServiceCollection services, Assembly assembly, IConsole console)
    {
        return services
                .AddScoped<IWebDownload, WebDownload>()
                .AddLocalFileSystem()
                .AddRemoteFileServiceAzBlob()
                .AddRemoteFileServiceGit()
                .AddRemoteFileServiceSmb()
                .AddRemoteFileServiceRclone()
                .RegisterInstructions()
                .RegisterConfiguration()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddScoped<INiPackageManagerService, NiPackageManagerService>()
                .AddScoped<IProcessService, ProcessService>()
                .AddScoped<ISnapshotSerializer, SnapshotSerializerJson>()
                .AddSingleton<IConsole>(console)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                })
                .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionAzBlob>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionAzBlob>))
                .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionSmb>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionSmb>))
                .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionGit>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionGit>))
                .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionRclone>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionRclone>))
                .AddScoped(typeof(IRequestHandler<AddDirectoryToRemoteFeedRequest<FeedDefinitionAzBlob>, Result>), typeof(AddDirectoryToRemoteFeedHandler<FeedDefinitionAzBlob>))
                .AddScoped(typeof(IRequestHandler<AddDirectoryToRemoteFeedRequest<FeedDefinitionSmb>, Result>), typeof(AddDirectoryToRemoteFeedHandler<FeedDefinitionSmb>))
                .AddScoped(typeof(IRequestHandler<AddDirectoryToRemoteFeedRequest<FeedDefinitionGit>, Result>), typeof(AddDirectoryToRemoteFeedHandler<FeedDefinitionGit>))
                .AddScoped(typeof(IRequestHandler<AddDirectoryToRemoteFeedRequest<FeedDefinitionRclone>, Result>), typeof(AddDirectoryToRemoteFeedHandler<FeedDefinitionRclone>));
        }

}

