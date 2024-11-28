using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

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
            var services = new ServiceCollection()
                .AddSingleton<IMyService, MyServiceImplementation>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .BuildServiceProvider();
            
            var builder = new GcdAppBuilder();
            var app = builder.Build(services);
            
            return app.Execute(args);
        }
    }
}




