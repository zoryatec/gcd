using Gcd.Commands.Project;
using Gcd.Commands.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Vipm;

public static class UseMenuVipmExt
{
    public const string NAME = "vipm";
    private static bool SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Commands for VIPM related tasks.";
    public static CommandLineApplication UseMenuVipm(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdKill(serviceProvider);
        });
        return app;
    }
}