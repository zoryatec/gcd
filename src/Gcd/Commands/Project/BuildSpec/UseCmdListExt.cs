using CSharpFunctionalExtensions;
using Gcd.Commands.Tools;
using Gcd.Extensions;
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

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var projectPath = new LabViewProjectPathOption();
            cmd.AddOptions(projectPath.IsRequired());
            cmd.OnExecute(async () =>
            {
                var maybeProjectPat = projectPath.Map();
                var request = new BuildSpecListRequest(maybeProjectPat.Value);
                var response = await mediator.Send(request);
                console.WriteLine(response.ProjectPaht);
            });
        });
        return app;
    }
}

