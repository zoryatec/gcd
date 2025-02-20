using Gcd.Commands.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project;

public static class UseMenuBuildSpecExt
{
    public const string NAME = "build-spec";
    public const string DESCRIPTION = "Commands related to LabVIEW project build specification.";
    public static CommandLineApplication UseBuildSpecCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdList(serviceProvider);
            cmd.UseCmdSetVersion(serviceProvider);
            cmd.UseCmdBuild(serviceProvider);
        });

        return app;
    }
}