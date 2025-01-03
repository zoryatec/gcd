using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseMenuToolsExt
{
    public static CommandLineApplication UseMenuTools(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("tools", cmd =>
        {
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdAddToUserPath(serviceProvider);
            cmd.UseCmdAddToSystemPath(serviceProvider);
            cmd.UseCmdInstallNipkg(serviceProvider);
            cmd.UseCmdDownloadNipkg(serviceProvider);
        });
        return app;
    }



}