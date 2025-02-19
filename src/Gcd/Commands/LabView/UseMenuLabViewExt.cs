using Gcd.Commands.LabView;
using Gcd.Commands.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project;

public static class UseMenuLabViewExt
{
    public const string NAME = "labview";
    private static bool SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Commands for LabVIEW related tasks.";
    public static CommandLineApplication UseLabViewCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseBuildSpecCmd(serviceProvider);
            cmd.UseCmdRunVi(serviceProvider);
            cmd.UseCmdBuildProject(serviceProvider);
            cmd.UseCmdKill(serviceProvider);
        });
        return app;
    }
}