using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd;

public class GcdAppBuilder
{
    public CommandLineApplication Build(IServiceProvider services)
    {
        var app = new CommandLineApplication<Program>()
        {
            Name = "gcd",
            Description = "CI/CD tool for G programmers with OCDddd",
        };
            
        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(services);

        var console = services.GetRequiredService<IConsole>();
        app.HelpOption(inherited: true);
        app.Command("config", configCmd =>
        {
            configCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                configCmd.ShowHelp();
                return 1;
            });

            configCmd.Command("set", setCmd =>
            {
                setCmd.Description = "Set config value";
                var key = setCmd.Argument("key", "Name of the config").IsRequired();
                var val = setCmd.Argument("value", "Value of the config").IsRequired();
                setCmd.OnExecute(() =>
                {
                    console.WriteLine($"Setting config {key.Value} = {val.Value}");
                });
            });

            configCmd.Command("list", listCmd =>
            {
                var json = listCmd.Option("--json", "Json output", CommandOptionType.NoValue);
                listCmd.OnExecute(() =>
                {
                    if (json.HasValue())
                    {
                        console.WriteLine("{\"dummy\": \"value\"}");
                    }
                    else
                    {
                        console.WriteLine("coreclationTest");
                    }
                });
            });
        });

        app.OnExecute(() =>
        {
            console.WriteLine("Specify a subcommand");
            app.ShowHelp();
            return 1;
        });

        return app;
    }
}