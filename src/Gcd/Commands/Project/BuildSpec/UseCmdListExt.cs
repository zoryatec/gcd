using CSharpFunctionalExtensions;
using Gcd.Handlers.Project.BuildSpec;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Project.BuildSpecList;

namespace Gcd.Commands.Project.BuildSpec;

public static class UseCmdListExt
{
    public static readonly string NAME = "list";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = false;
    public static readonly string DESCRIPTION = "list";
    
    public static CommandLineApplication UseCmdList(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, listCmd =>
        {
            listCmd.Description = DESCRIPTION;
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

