using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Gcd.Services;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Model.Config;
using Microsoft.Extensions.Configuration;
using Gcd.Services.DI;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Commands;
using Gcd.Model.Nipkg.FeedDefinition;


namespace Gcd
{
    public class Program
    {
        private readonly IConsole _console;

        public Program(IConsole console)
        {
            _console = console;
        }
        public static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddScoped<IWebDownload, WebDownload>()
                .AddScoped<IFileSystem, LocalFileService>()
                .AddScoped<IRemoteFileSystemAzBlob,RemoteFileSystemAzBlob>()
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

            var serviceProvider = services.BuildServiceProvider();
            var console = serviceProvider.GetRequiredService<IConsole>();

            var app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };
            
            app.UseMenuGcd(serviceProvider);

            try
            {
                return app.Execute(args);
            }
            catch (Exception ex)
            {
                console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return 11; // Return a global error code
            }
        }
        
        private void OnExecuteVersionizeCommand()
        {
        }
    }
}




