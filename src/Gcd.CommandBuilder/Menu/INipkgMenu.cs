using Gcd.CommandBuilder.Menu.Nipkg;

namespace Gcd.CommandBuilder.Menu;

public interface INipkgMenu
{
    public IFeedSmb WithFeedSmbMenu();
    public IFeedLocal WithFeedLocalMenu();
    public IFeedAzBlob WithAzBlobFeedMenu();
    public IFeedGit WithFeedGitdMenu();
    public IFeedRclone WithFeedRcloneMenu();
    public IBuilderMenu WithBuilderMenu();
    public global::Gcd.CommandBuilder.Command.Nipkg.ArgBuilderBuild WithBuildCmd();
    
    public global::Gcd.CommandBuilder.Command.Nipkg.ArgBuilderExport WithExportCmd();
}