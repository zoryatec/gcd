using Gcd.CommandHandlers;
using Gcd.Extensions;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Gcd.Services;


namespace Gcd
{
    public class Program
    {
        private readonly IConsole _console;
        private readonly IVersionizeCommandHandler _versionizeCommandHandler;

        public Program(IConsole console,
            IVersionizeCommandHandler versionizeCommandHandler
        )
        {
            _console = console;
            _versionizeCommandHandler = versionizeCommandHandler;
        }
        public static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
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
            _versionizeCommandHandler.Handle();
        }
    }
}




