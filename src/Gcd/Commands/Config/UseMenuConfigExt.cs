using Gcd.Commands.Config;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

public static class UseMenuConfigExt
{
    private static bool SHOW_IN_HELP = false;
    private static string NAME = "config";
    public static CommandLineApplication UseMenuConfig(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = false;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdSet(serviceProvider);
            cmd.UseCmdGet(serviceProvider);

        });
        return app;
    }
}