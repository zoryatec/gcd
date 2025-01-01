using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.LabViewProject;
using Gcd.Menu;
using Gcd.Model.Config;
using Gcd.Services;
using Gcd.Services.DI;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests
{
    public class GcdProcessApp : IGcdProcess
    {
        IConsole _console;
        CommandLineApplication _app; 
        public GcdProcessApp()
        {
            _console = new FakeConsole();
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddSingleton<IWebDownload, WebDownload>()
                .AddScoped<IFileSystem, LocalFileService>()
                .AddScoped<IRemoteFileSystem, RemoteFileSystem>()
                .AddScoped<RemoteFileSystemSmb, RemoteFileSystemSmb>()
                .AddScoped<RemoteFileSystemGit, RemoteFileSystemGit>()
                .RegisterInstructions()
                .RegisterConfiguration()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddSingleton<IConsole>(_console)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                });

            var serviceProvider = services.BuildServiceProvider();

            _app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };

            _app.UseGcdCmd(serviceProvider);
        }

        public GcdProcessResponse Run(string[] request)
        {
            return Run(new GcdProcessRequest() { Arguments = request });
        }
        public GcdProcessResponse Run(GcdProcessRequest request)
        {
            var result = _app.ExecuteAsync(request.Arguments).GetAwaiter().GetResult();

            return new GcdProcessResponse
            {
                Error = _console.Error.ToString(),
                Out = _console.Out.ToString(),
                Return = result
            };
        }
    }
}
