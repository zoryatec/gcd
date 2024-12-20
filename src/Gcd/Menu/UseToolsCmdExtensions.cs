using Gcd.Commands.Tools.AddToPath;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Menu;

public static class UseSystemExtensions
{
    public static CommandLineApplication UseToolsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("tools", systemCmd =>
        {
            systemCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                systemCmd.ShowHelp();
                return 1;
            });
            systemCmd.UseAddToUserPathCmd(serviceProvider);
            systemCmd.UseAddToSystemPathCmd(serviceProvider);
        });
        return app;
    }



}