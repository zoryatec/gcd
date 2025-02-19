using CSharpFunctionalExtensions;
using Gcd.Commands.Tools;
using Gcd.Extensions;
using Gcd.Handlers.LabView;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.LabView;

public static class UseBuildProjectExt
{
    public static readonly string NAME = "build-project";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = false;
    private static string DESCRIPTION = "Command builds all (supported) build specifications from the project with provided version and output path.";
    
    public static CommandLineApplication UseCmdBuildProject(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var labviewProjectPathOption = new LabViewProjectPathOption();
            var lvPathOption = new LabViewPathOption();
            var lvPortOption = new LabViewPortOption();
            var projectOutputDirOption = new ProjectOutputDirOption();
            var projectVersionOption = new LabViewProjectVersionOption();
            cmd.AddOptions(
                labviewProjectPathOption.IsRequired(),
                lvPathOption.IsRequired(),
                lvPortOption.IsRequired(),
                projectOutputDirOption.IsRequired(),
                projectVersionOption.IsRequired()
                );
            
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var lvProjectPath = labviewProjectPathOption.Map();
                var lvPath = lvPathOption.Map();
                var lvPort = lvPortOption.Map();
                var projectOutputDir = projectOutputDirOption.Map();
                var projectVersion = projectVersionOption.Map();

                
                var response = await mediator.ProjectBuildAsync(lvProjectPath.Value,
                    lvPath.Value,
                    lvPort.Value,
                    projectOutputDir.Value,
                    projectVersion.Value,
                    cancellationToken: cancelationToken);

                
                return response
                    .Tap((rsp) => console.Write(rsp))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

