namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IFeedLocal
{
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddLocalPackage WithAddLocalPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddHttpPackage WithAddHttpPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedLocal.ArgBuilderAddLocalDirectory WithAddLocalDirectoryCmd();
}