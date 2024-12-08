using System.Text.Json;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.Commands.ProjectBuildSpecSetVersion;
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
        app.Command("build-spec", buildSpecCmd =>
        {
            buildSpecCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                buildSpecCmd.ShowHelp();
                return 1;
            });
            buildSpecCmd.UseBuildSpecListCmd(serviceProvider);
            buildSpecCmd.UseBuildSpecSetVersionCmd(serviceProvider);
        });

        return app;
    }

    public static CommandLineApplication UseBuildSpecListCmd(this CommandLineApplication app,
        IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("list", listCmd =>
        {
            listCmd.Description = "List build specificatnions";
            var projectPath = listCmd.Option("--project-path", "Absolute path to a project", CommandOptionType.SingleValue)
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

    public static CommandLineApplication UseBuildSpecSetVersionCmd(this CommandLineApplication app,
    IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("set-version", listCmd =>
        {
            listCmd.Description = "Set version of build specification";
            var projectPath = listCmd.Option("--project-path", "Absolute path to a project", CommandOptionType.SingleValue)
                .IsRequired();
            var buildSpecName = listCmd.Option("--build-spec-name", "Build specification name", CommandOptionType.SingleValue)
                .IsRequired();
            var buildSpecType = listCmd.Option("--build-spec-type", "Build specification type", CommandOptionType.SingleValue)
                .IsRequired();
            var buildSpecTarget = listCmd.Option("--build-spec-target", "Build specification target", CommandOptionType.SingleValue)
                .IsRequired();
            var version = listCmd.Option("--version", "Version to be set", CommandOptionType.SingleValue)
                .IsRequired();

            listCmd.OnExecuteAsync(async cancelationToken =>
            {
                var maybeProjectPath = Maybe.From(projectPath.Value()).ToResult("Projectpath not found");
                var maybebuildSpecName = Maybe.From(buildSpecName.Value()).ToResult("buildsepecname not found");
                var maybebuildSpecType = Maybe.From(buildSpecType.Value()).ToResult("buildspectype not found");
                var maybebuildSpecTarget = Maybe.From(buildSpecTarget.Value()).ToResult("build spec target not found");
                var maybeversion = Maybe.From(version.Value()).ToResult("version not found");

                var result = Result.Combine(maybeProjectPath,
                        maybebuildSpecName,
                        maybebuildSpecType,
                        maybebuildSpecTarget,
                        maybeversion);

                if (result.IsSuccess)
                {
                    var request = new BuildSpecSetVersionRequest(maybeProjectPath.Value, maybebuildSpecName.Value, maybebuildSpecType.Value, maybebuildSpecTarget.Value, maybeversion.Value);
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                    return 0;
                }
                else 
                { 
                    console.Error.Write(result.Error);
                    return 1;
                }

            });
        });
        return app;
    }
}