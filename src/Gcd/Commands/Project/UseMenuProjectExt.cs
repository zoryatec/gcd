using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project;

public static class UseMenuProjectExt
{
    private static bool SHOW_IN_HELP = false;
    public static CommandLineApplication UseProjectCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("project", cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseBuildSpecCmd(serviceProvider);
        });
        return app;
    }
}