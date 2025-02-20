using Gcd.Commands;
using Gcd.DI;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.Fixture
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
                .AddGcd(assembly, _console);

            var serviceProvider = services.BuildServiceProvider();

            _app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };

            _app.UseMenuGcd(serviceProvider);
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
                Error = _console.Error.ToString() ?? string.Empty,
                Out = _console.Out.ToString() ?? string.Empty,
                Return = result
            };
        }
    }
}
