using CSharpFunctionalExtensions;
using Gcd.Commands.LabView;
using Gcd.Commands.Tools;
using Gcd.Extensions;
using Gcd.Handlers.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project.BuildSpec;

public static class UseCmdSetVersionExt
{
    public const string NAME = "set-version";
    public const string DESCRIPTION = "Command sets the version of LabVIEW project build specification";
    public static CommandLineApplication UseCmdSetVersion(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        //const string SUCESS_MESSAGE = "Metadata pushed successully";

        app.Command(NAME, listCmd =>
        {
            listCmd.Description = DESCRIPTION;
            var buildSpecType = listCmd.Option("--build-spec-type", "Build specification type", CommandOptionType.SingleValue)
                .IsRequired();
            var buildSpecVersionOption = new LabViewBuildSpecVersionOption();
            var buildSpecNameOption = new LabViewBuildSpecNameOption();
            var buildSpecTargetOption = new LabViewBuildSpecTargetOption();
            var projectPathOption = new LabViewProjectPathOption();

            listCmd.AddOptions(
                buildSpecVersionOption.IsRequired(),
                projectPathOption.IsRequired(),
                buildSpecNameOption.IsRequired(),
                buildSpecTargetOption.IsRequired()
            );

            listCmd.OnExecuteAsync(async cancelationToken =>
            {
                var projectPath = projectPathOption.Map();
                var buildSpecTarget = buildSpecTargetOption.Map();
                var maybebuildSpecType = Maybe.From(buildSpecType.Value()).ToResult("buildspectype not found");
                var buildSpecName = buildSpecNameOption.Map();
                var buildSpecVersion = buildSpecVersionOption.Map();

                
                var response = await mediator.BuildSpecSetVersionAsync(projectPath.Value, buildSpecName.Value, maybebuildSpecType.Value,buildSpecTarget.Value, buildSpecVersion.Value);
                return response
                    .Tap(() => console.Write("sucess "))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

