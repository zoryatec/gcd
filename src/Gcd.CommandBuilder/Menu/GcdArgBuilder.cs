using Gcd.CommandBuilder.Command.Config;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Command.Tools;
using Gcd.CommandBuilder.Command.Vipm;
using Gcd.CommandBuilder.Menu.LabView;
using Gcd.CommandBuilder.Menu.Nipkg;
using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedGit;
using Gcd.Commands.Nipkg.FeedGitHub;
using Gcd.Commands.Nipkg.FeedLocal;
using Gcd.Commands.Nipkg.FeedRclone;
using Gcd.Commands.Nipkg.FeedSmb;
using Gcd.Commands.Project;
using Gcd.Commands.Tools;
using Gcd.Commands.Vipm;
using Gcd.LabViewProject;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Menu;


public class GcdArgBuilder(Arguments arguments) :
    ArgBuilder(arguments),
    IGcdMenu,
    INipkgMenu,
    IFeedLocal,
    IFeedSmb,
    IFeedAzBlob,
    IVipmMenu,
    IBuilderMenu,
    IFeedGit,
    IFeedGitHub,
    IFeedRclone,
    IToolsMenu,
    ILabViewMenu,
    IBuildSpecMenu,
    IConfigMenu
{
    public GcdArgBuilder() : this(new Arguments()) { }
    
    public static IGcdMenu Create() =>
    (IGcdMenu) new GcdArgBuilder();
    
    // gcd
    public IToolsMenu WithToolsMenu() =>
        (GcdArgBuilder) WithArg(UseMenuToolsExt.NAME);
    
    public IVipmMenu WithVipmMenu() =>
        (GcdArgBuilder) WithArg(UseMenuVipmExt.NAME);

    public INipkgMenu WithNipkgMenu() =>
        (GcdArgBuilder) WithArg(UseMenuNipkgExtension.NAME);  
    
    public ILabViewMenu WithLabViewMenu() =>
        (GcdArgBuilder) WithArg(UseMenuLabViewExt.NAME);

    public IConfigMenu WithConfigMenu() =>
        (GcdArgBuilder) WithArg("config"); 

    // labview
    public IBuildSpecMenu WithBuildSpecMenu() =>
        (GcdArgBuilder) WithArg(UseMenuBuildSpecExt.NAME);  

    
    // nipkg feed menu
    public IFeedSmb WithFeedSmbMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedSmbExt.NAME);
    public IFeedLocal WithFeedLocalMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedLocalExt.NAME);
    public IFeedAzBlob WithAzBlobFeedMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedAzBlobExt.NAME);
    public IFeedGit WithFeedGitdMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedGitlExt.NAME);
    
    public IFeedGitHub WithFeedGitHubMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedGitHublExt.NAME);
    
    public IFeedRclone WithFeedRcloneMenu() =>
        (GcdArgBuilder) WithArg(UseMenuFeedRcloneExt.NAME);

    public IBuilderMenu WithBuilderMenu() =>
        (GcdArgBuilder) WithArg(UseMenuBuilderExt.NAME);
    
    // nipkg builder
    global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderAddContent IBuilderMenu.WithAddContentCmd()=>
        new(_arguments);

    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderInit WithInitCmd() =>
        new(_arguments);

    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderSetVersion WithSetVersionCmd() =>
        new(_arguments);

    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderAddInstruction WithAddInstructionCmd() =>
        new(_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.ArgBuilderBuild INipkgMenu.WithBuildCmd() =>
        new(_arguments);
    
    global::Gcd.CommandBuilder.Command.Nipkg.ArgBuilderExport INipkgMenu.WithExportCmd() =>
        new(_arguments);

    // nipkg feed local menu
    global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddLocalPackage IFeedLocal.WithAddLocalPackageCmd() =>
         new (_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddHttpPackage IFeedLocal.WithAddHttpPackageCmd() =>
        new (_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddLocalDirectory IFeedLocal.WithAddLocalDirectoryCmd() =>
        new (_arguments);

    // nipkg feed az blob menu
    global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderPullMetaData IFeedAzBlob.WithPullMetaDataCmd() =>
        new (_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderPushMetaData IFeedAzBlob.WithPushMetaDataCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderAddLocalPackage IFeedAzBlob.WithAddLocalPackageCmd() =>
        new (_arguments);

    // nipkg feed smb menu
    global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderPullMetaData IFeedSmb.WithPullMetaDataCmd() =>
        new (_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderPushMetaData IFeedSmb.WithPushMetaDataCmd() =>
        new (_arguments);

    global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderAddLocalPackage IFeedSmb.WithAddLocalPackageCmd() =>
        new (_arguments);
    
    global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderAddLocalDirectory IFeedSmb.WithAddLocalDirectoryCmd() =>
        new (_arguments);
    
    // git
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderAddLocalPackage IFeedGit.WithAddLocalPackageCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderPullMetaData IFeedGit.WithPullMetaDataCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderPushMetaData IFeedGit.WithPushMetaDataCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderAddHttpPackage IFeedGit.WithAddHttpPackageCmd() =>
        new (_arguments);
    
    // github
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderAddLocalPackage IFeedGitHub.WithAddLocalPackageCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderPullMetaData IFeedGitHub.WithPullMetaDataCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderPushMetaData IFeedGitHub.WithPushMetaDataCmd() =>
        new (_arguments);
    global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderAddHttpPackage IFeedGitHub.WithAddHttpPackageCmd() =>
        new (_arguments);
    
    // nipkg feed rclone
    global::Gcd.CommandBuilder.Command.Nipkg.FeedRclone.ArgBuilderAddLocalPackage IFeedRclone.WithAddLocalPackageCmd() =>
        new (_arguments);
    
    global::Gcd.CommandBuilder.Command.Nipkg.FeedRclone.ArgBuilderAddLocalDirectory IFeedRclone.WithAddLocalDirectoryCmd() =>
        new (_arguments);
    
    
   // vipm menu
    public ArgBuilderVipmKill WithKillCmd() =>
        new (_arguments);



    // tools
    public Gcd.CommandBuilder.Command.Tools.ArgBuilderAddToUserPath WithAddToUserPathCmd() =>
        new (_arguments);

    public Gcd.CommandBuilder.Command.Tools.ArgBuilderDownloadNipkg WithDownloadNipkgCmd() =>
        new (_arguments);

    public Gcd.CommandBuilder.Command.Tools.ArgBuilderInstallNipkg WithInstallNipkgCmd() =>
        new (_arguments);

    // labview
    ArgBuilderLabViewKill ILabViewMenu.WithKillCmd() =>
        new (_arguments);
    
    ArgBuilderRunVi ILabViewMenu.WithRunViCmd() =>
        new (_arguments);
    
    ArgBuilderBuildProject ILabViewMenu.WithBuildProjectCmd() =>
        new (_arguments);
    
    // build spec
    
    ArgBuilderBuildSpec IBuildSpecMenu.WithBuildCmd() =>
        new (_arguments);
    
    ArgBuilderSetVersion IBuildSpecMenu.WithSetVersionCmd() =>
        new (_arguments);
    
    ArgBuilderList IBuildSpecMenu.WithListCmd() =>
        new (_arguments);
    
    // config
    
    public ArgBuilderGetConfig WithGetCmd() =>
        new (_arguments);
    public ArgBuilderSetConfig WithSetCmd() =>
        new (_arguments);
    
}
