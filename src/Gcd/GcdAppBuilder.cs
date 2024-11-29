using System.Reflection;
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
        app.VersionOption("-v|--version", GetVersion());
        
        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(services);
        app.HelpOption(inherited: true);
        
        var console = services.GetRequiredService<IConsole>();
        var projectService = services.GetRequiredService<IProjectService>();
        

        #region versionize
        app.Command("versionize", versionizeCommand =>
        {
            versionizeCommand.OnExecute(() =>
            {
                var handler = services.GetRequiredService<IVersionizeCommandHandler>();
                handler.Handle();
            });
        });
        #endregion
        
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
    private static string GetVersion() => typeof(Program).Assembly.GetName().Version?.ToString() ?? "";
}