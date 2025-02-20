using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Commands;
using Gcd.DI;


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
                .AddGcd(assembly,PhysicalConsole.Singleton);

            var serviceProvider = services.BuildServiceProvider();
            var console = serviceProvider.GetRequiredService<IConsole>();

            var app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "(G) LabVIEW CI/CD tool OR tool for (G) LabVIEW programmer with oCD.",
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




