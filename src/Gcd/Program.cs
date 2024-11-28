using Gcd.CommandHandlers;
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
                .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .BuildServiceProvider();
            
            var builder = new GcdAppBuilder();
            var app = builder.Build(services);
            
            return app.Execute(args);
        }
        
        private void OnExecuteVersionizeCommand()
        {
            _versionizeCommandHandler.Handle();
        }
    }
}




