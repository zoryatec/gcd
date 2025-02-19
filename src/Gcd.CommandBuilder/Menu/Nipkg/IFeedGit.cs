namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IFeedGit
{
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderAddLocalPackage WithAddLocalPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderAddHttpPackage WithAddHttpPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderPullMetaData WithPullMetaDataCmd(); 
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGit.ArgBuilderPushMetaData WithPushMetaDataCmd(); 
}