using System.Text.Json;
using Gcd.CommandHandlers;
using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
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
        var projectService = serviceProvider.GetRequiredService<IProjectService>();
        app.Command("build-spec", buildSpecCmd =>
        {
            buildSpecCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                buildSpecCmd.ShowHelp();
                return 1;
            });
            buildSpecCmd.UseBuildSpecListCmd(serviceProvider);
        });

        return app;
    }

    public static CommandLineApplication UseBuildSpecListCmd(this CommandLineApplication app,
        IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var projectService = serviceProvider.GetRequiredService<IProjectService>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Description = "Related to build specifications";
        app.Command("list", listCmd =>
        {
            listCmd.Description = "List build specificatnions";
            var projectPath = listCmd.Option("--project-path", "Json output", CommandOptionType.SingleValue)
                .IsRequired();
            listCmd.OnExecute(async () =>
            {
                var request = new BuildSpecListRequest(projectPath.Value());
                var response =  await mediator.Send(request);
                console.WriteLine(response.ProjectPaht);
            });
        });
        return app;
    }
}