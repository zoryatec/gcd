using CSharpFunctionalExtensions;
using Gcd.Commands.LabView;
using Gcd.Commands.Tools;
using Gcd.Extensions;
using Gcd.Handlers.LabView;
using Gcd.Handlers.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Project.BuildSpec;

public static class UseCmdBuildExt
{
    public static readonly string NAME = "build";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = false;
    public const string DESCRIPTION = "Command builds LabVIEW project build specification with provided version and output path.";
    
    public static CommandLineApplication UseCmdBuild(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var projectPath = new LabViewProjectPathOption();
            var lvPathOption = new LabViewPathOption();
            var lvPortOption = new LabViewPortOption();
            var buildSpecNameOption = new LabViewBuildSpecNameOption();
            var buildSpecTargetOption = new LabViewBuildSpecTargetOption();
            var buildSpecVersionOption = new LabViewBuildSpecVersionOption();
            var buildSpecOutputDirOption = new LabViewBuildSpecOutputDirOption();
            cmd.AddOptions(
                projectPath.IsRequired(),
                lvPathOption.IsRequired(),
                lvPortOption.IsRequired(),
                buildSpecNameOption.IsRequired(),
                buildSpecTargetOption.IsRequired(),
                buildSpecVersionOption.IsRequired(),
                buildSpecOutputDirOption.IsRequired()
                );
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var lvProjPath = projectPath.Map();
                var lvPath = lvPathOption.Map();
                var lvPort = lvPortOption.Map();
                var buildSpecName = buildSpecNameOption.Map();
                var buildSpecTarget = buildSpecTargetOption.Map();
                var buildSpecVersion = buildSpecVersionOption.Map();
                var buildSpecOutputDir = buildSpecOutputDirOption.Map();
                
                var response = await mediator.BuildSpecBuildAsync(
                    lvProjPath.Value,
                    lvPath.Value,
                    lvPort.Value,
                    buildSpecName.Value,
                    buildSpecTarget.Value,
                    buildSpecVersion.Value,
                    buildSpecOutputDir.Value);
                return response
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

