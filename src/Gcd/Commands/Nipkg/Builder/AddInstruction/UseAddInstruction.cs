using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Extensions;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Commands.Nipkg.Builder.Init;


namespace Gcd.Commands.Nipkg.Builder.AddInstruction;

public static class UseAddInstructionCmdExtensions
{
    public static CommandLineApplication UseAddInstructionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command("add-instruction", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            var builderRootDirArg = new PackageBuilderRootDirArgument();
            command.AddArgument(builderRootDirArg.IsRequired());



            command.OnExecuteAsync(async cancelationToken =>
            {
                //var value = builderRootDirArg.Value;
                //var builderRootDir = builderRootDirArg.Map();
                //var contentSrcDir = contentSrcDirOpt.Map();
                //var targetDir = targetDirOpt.Map();

                //return await Result
                //    .Combine(builderRootDir, contentSrcDir, targetDir)
                //    .Map(() => ContentLink.Of(targetDir.Value, contentSrcDir.Value))
                //    .Bind((link) => mediator.AddContentAsync(builderRootDir.Value, link, cancelationToken))
                //    .Tap(() => console.Write("SUCESS_MESSAGE"))
                //    .TapError(error => console.Error.Write(error))
                //    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}