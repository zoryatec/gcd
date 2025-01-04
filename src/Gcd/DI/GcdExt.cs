using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LabViewProject;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services;
using Gcd.Services.DI;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.DI;

public static class GcdExt
{

    public static IServiceCollection AddGcd(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        return services
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddScoped<IWebDownload, WebDownload>()
                .AddScoped<IFileSystem, LocalFileService>()
                .AddScoped<IRemoteFileSystemAzBlob, RemoteFileSystemAzBlob>()
                .AddScoped<RemoteFileSystemSmb, RemoteFileSystemSmb>()
                .AddScoped<RemoteFileSystemGit, RemoteFileSystemGit>()
                .RegisterInstructions()
                .RegisterConfiguration()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                })
               .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionAzBlob>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionAzBlob>))
               .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionGit>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionGit>));
    }

}

