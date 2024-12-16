using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.LabViewProject;
using Gcd.Menu;
using Gcd.Model.Config;
using Gcd.Services;
using Gcd.Tests.EndToEnd;
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
            var nipkgInstaller = "NIPackageManager21.3.0_online.exe";
            var url = $"https://download.ni.com/support/nipkg/products/ni-package-manager/installers/{nipkgInstaller}";
            var nipkgCmdPath = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddScoped<IWebDownload, WebDownload>()
                .AddScoped<IFileSystem, LocalFileService>()
                .AddScoped<IControlPropertyFactory, ControlPropertyFactory>()
                .AddScoped<NipkgInstallerUri>(x => NipkgInstallerUri.Of(url).Value)
                .AddScoped<NipkgCmdPath>(x => NipkgCmdPath.Of(nipkgCmdPath).Value)
                .AddScoped<ITempDirectoryProvider, TempDirectoryProvider>()
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
            var result = _app.Execute(request.Arguments);

            return new GcdProcessResponse
            {
                Error = _console.Error.ToString(),
                Out = _console.Out.ToString(),
                Return = result
            };
        }
    }
}
