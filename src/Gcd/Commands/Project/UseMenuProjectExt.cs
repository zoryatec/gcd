using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project;

public static class UseMenuProjectExt
{
    public static CommandLineApplication UseProjectCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("project", projectCmd =>
        {
            projectCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                projectCmd.ShowHelp();
                return 1;
            });
            projectCmd.UseBuildSpecCmd(serviceProvider);
        });
        return app;
    }
}