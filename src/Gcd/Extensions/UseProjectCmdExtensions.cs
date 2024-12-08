using Gcd.Commands.NipkgDownloadFeedMetaData;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Extensions;

public static class UseProjectCmdExtensions
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

    public static CommandLineApplication UseBuildSpecCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("build-spec", buildSpecCmd =>
        {
            buildSpecCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                buildSpecCmd.ShowHelp();
                return 1;
            });
            buildSpecCmd.UseBuildSpecListCmdd(serviceProvider);
            buildSpecCmd.UseBuildSpecSetVersionCmd(serviceProvider);
        });

        return app;
    }
}