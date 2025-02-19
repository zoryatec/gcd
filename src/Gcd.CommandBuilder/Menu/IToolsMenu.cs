namespace Gcd.CommandBuilder.Menu;

public interface IToolsMenu
{
    public Gcd.CommandBuilder.Command.Tools.ArgBuilderAddToUserPath WithAddToUserPathCmd();
    public Gcd.CommandBuilder.Command.Tools.ArgBuilderDownloadNipkg WithDownloadNipkgCmd();
    public Gcd.CommandBuilder.Command.Tools.ArgBuilderInstallNipkg WithInstallNipkgCmd();
}