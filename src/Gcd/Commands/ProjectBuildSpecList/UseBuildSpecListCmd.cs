using CSharpFunctionalExtensions;
using Gcd.Commands.ProjectBuildSpecList;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public static class UseBuildSpecListCmdExtensions
{
    public static CommandLineApplication UseBuildSpecListCmdd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        //const string SUCESS_MESSAGE = "Metadata pushed successully";

        app.Command("list", listCmd =>
        {
            listCmd.Description = "List build specificatnions";
            var projectPath = listCmd.Option("--project-path", "Absolute path to a project", CommandOptionType.SingleValue)
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

