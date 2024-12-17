using Gcd.Commands.Config.SetConfig;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

public static class UseConfigCmdExtensions
{
    public static CommandLineApplication UseConfigCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("config", cmd =>
        {
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseSetConfigCmd(serviceProvider);

        });
        return app;
    }
}