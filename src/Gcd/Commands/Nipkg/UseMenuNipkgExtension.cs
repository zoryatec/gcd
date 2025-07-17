using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Commands.Nipkg.Build;
using Gcd.Commands.Nipkg.FeedGit;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedLocal;
using Gcd.Commands.Nipkg.FeedRclone;
using Gcd.Commands.Nipkg.FeedSmb;
using Gcd.Commands.Nipkg.InstallFromInstallerDirectory;
using Gcd.Commands.Nipkg.InstallFromInstallerIso;
using Gcd.Commands.Nipkg.InstallFromSnapshot;


namespace Gcd.Commands.Nipkg;

public static class UseMenuNipkgExtension
{
    public static readonly string NAME = "nipkg";
    private static string DESCRIPTION = "Commands for NIPM/NIPKG related tasks.";
    public static CommandLineApplication UseMenuNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseMenuBuilder(serviceProvider);
            cmd.UseMenuFeedAzBlob(serviceProvider);
            cmd.UseMenuFeedLocal(serviceProvider);
            cmd.UseMenuFeedGit(serviceProvider);
            cmd.UseMenuFeedSmb(serviceProvider);
            cmd.UseMenuFeedRclone(serviceProvider);
            cmd.UseCmdBuild(serviceProvider);
            cmd.UseCmdInstallFromSnapshot(serviceProvider);
            cmd.UseCmdInstallFromInstallerDirectory(serviceProvider);
            cmd.UseCmdInstallFromInstallerIso(serviceProvider);
            cmd.UseCmdExport(serviceProvider);
        });
        return app;
    }


    public static CommandLineApplication UseFilePackageInstructionsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("instructions-file-pkg", cmd =>
        {
            cmd.ShowInHelpText = false;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddInstruction(serviceProvider);

        });

        return app;
    }

    public static CommandLineApplication UseMsiPackageInstructionsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("instructions-msi-pkg", cmd =>
        {

            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

        });

        return app;
    }

}

