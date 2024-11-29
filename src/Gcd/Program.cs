using Gcd.CommandHandlers;
using Gcd.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

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
            var services = new ServiceCollection()
                .AddSingleton<IProjectService, ProjectService>()
                .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .BuildServiceProvider();
            
            var app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };
            
            app.UseGcdCmd(services);
            
            return app.Execute(args);
        }
        
        private void OnExecuteVersionizeCommand()
        {
            _versionizeCommandHandler.Handle();
        }
    }
}




