using Gcd.Commands.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project;

public static class UseMenuBuildSpecExt
{
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
            buildSpecCmd.UseCmdList(serviceProvider);
            buildSpecCmd.UseCmdSetVersion(serviceProvider);
        });

        return app;
    }
}