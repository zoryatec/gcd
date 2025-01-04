using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
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
                .RegisterInstructions()
                .RegisterConfiguration()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddSingleton<IConsole>(console)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                })
               .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionAzBlob>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionAzBlob>))
               .AddScoped(typeof(IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionGit>, Result>), typeof(AddPackageToRemoteFeedHandler<FeedDefinitionGit>));
    }

}

