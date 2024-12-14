
using Gcd.Extensions;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Gcd.Services;
using Gcd.Model;
using Gcd.Commands.NipkgPackageBuilserSetVersion;


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

            var nipkgInstaller = "NIPackageManager21.3.0_online.exe";
            var url = $"https://download.ni.com/support/nipkg/products/ni-package-manager/installers/{nipkgInstaller}";
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddScoped<IWebDownload, WebDownload>()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<NipkgInstallerUri>(x => NipkgInstallerUri.Of(url).Value)
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                });
            
            var serviceProvider = services.BuildServiceProvider();
            var console = serviceProvider.GetRequiredService<IConsole>();

            var app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };
            
            app.UseGcdCmd(serviceProvider);

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




