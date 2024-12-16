using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Project.BuildSpecList;

namespace Gcd.Commands.Project.BuildSpec.List;

public static class UseBuildSpecListCmdExtensions
{
    public static CommandLineApplication UseBuildSpecListCmdd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, listCmd =>
        {
            listCmd.Description = COMMAND_DESCRIPTION;
            var projectPath = listCmd.Option(PROJECT_PATH_OPTION, PROJECT_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            listCmd.OnExecute(async () =>
            {
                var maybeProjectPat = Maybe.From(projectPath.Value());
                var request = new BuildSpecListRequest(maybeProjectPat.Value);
                var response = await mediator.Send(request);
                console.WriteLine(response.ProjectPaht);
            });
        });
        return app;
    }
}

