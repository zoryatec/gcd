using System.Reflection;
using System.Text.Json;
using Gcd.CommandHandlers;
using Gcd.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Gcd.LabViewProject;

namespace Gcd;

public class GcdAppBuilder()
{
    public CommandLineApplication Build(IServiceProvider services)
    {
        var console = services.GetRequiredService<IConsole>();
        
        var app = new CommandLineApplication<Program>()
        {
            Name = "gcd",
            Description = "CI/CD tool for G programmers with OCDddd",
        };

        return app.UseGcdCmd(services);
    }
}