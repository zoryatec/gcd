using Gcd.Commands.Config;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

public static class UseMenuConfigExt
{
    public static CommandLineApplication UseMenuConfig(this CommandLineApplication app, IServiceProvider serviceProvider)
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
            cmd.UseCmdSet(serviceProvider);
            cmd.UseCmdGet(serviceProvider);

        });
        return app;
    }
}