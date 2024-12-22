using Gcd.Commands.Tools.AddToPath;
using Gcd.Commands.Tools.DownloadNipkg;
using Gcd.Commands.Tools.InstallNipkg;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Menu;

public static class UseSystemExtensions
{
    public static CommandLineApplication UseToolsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
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
            cmd.UseAddToUserPathCmd(serviceProvider);
            cmd.UseAddToSystemPathCmd(serviceProvider);
            cmd.UseInstallNipkgCmd(serviceProvider);
            cmd.UseDownloadNipkgCmd(serviceProvider);
        });
        return app;
    }



}