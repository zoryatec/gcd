using Gcd.CommandHandlers;
using Gcd.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MediatR;


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
                .AddSingleton<IProjectService, ProjectService>()
                .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                });
            
            var serviceProvider = services.BuildServiceProvider();
            
            
            var app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };
            
            app.UseGcdCmd(serviceProvider);
            
            return app.Execute(args);
        }
        
        private void OnExecuteVersionizeCommand()
        {
            _versionizeCommandHandler.Handle();
        }
    }
}




