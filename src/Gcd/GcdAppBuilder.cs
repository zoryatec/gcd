using System.Text.Json;
using Gcd.CommandHandlers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Gcd.LabViewProject;

namespace Gcd;

public class GcdAppBuilder()
{
    public CommandLineApplication Build(IServiceProvider services)
    {
        var app = new CommandLineApplication<Program>()
        {
            Name = "gcd",
            Description = "CI/CD tool for G programmers with OCDddd",
        };
            
        var maybyProject = LabViewProject.LabViewProject.Create("");
        var project = maybyProject;
        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(services);

        var console = services.GetRequiredService<IConsole>();
        var projectService = services.GetRequiredService<IProjectService>();
        app.HelpOption(inherited: true);
        
        app.Command("versionize", versionizeCommand =>
        {
            versionizeCommand.OnExecute(() =>
            {
                var handler = services.GetRequiredService<IVersionizeCommandHandler>();
                handler.Handle();
            });
        });
        
        #region project
        app.Command("project", projectCmd =>
        {
            projectCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                projectCmd.ShowHelp();
                return 1;
            });

            projectCmd.Command("build-spec", buildSpecCmd =>
            {
                buildSpecCmd.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    buildSpecCmd.ShowHelp();
                    return 1;
                });
                
                buildSpecCmd.Description = "Related to build specifications";
                buildSpecCmd.Command("list", listCmd =>
                {
                    listCmd.Description = "List build specificatnions";
                    var projectPath = listCmd.Option("--project-path", "Json output", CommandOptionType.SingleValue)
                        .IsRequired();
                    listCmd.OnExecute(() =>
                    {
                        var projectListDto = projectService.GetBuildSpecList(projectPath.Value());
                        string jsonString = JsonSerializer.Serialize(projectListDto);
                        console.WriteLine(jsonString);
                    });
                });
            });

            projectCmd.Command("list-build-spec", listCmd =>
            {
                var json = listCmd.Option("--json", "Json output", CommandOptionType.NoValue);
                listCmd.OnExecute(() =>
                {
                    if (json.HasValue())
                    {
                        console.WriteLine("{\"dummy\": \"value\"}");
                    }
                    else
                    {
                        console.WriteLine("coreclationTest");
                    }
                });
            });
        });
        #endregion

        app.OnExecute(() =>
        {
            console.WriteLine("Specify a subcommand");
            app.ShowHelp();
            return 1;
        });

        return app;
    }
}