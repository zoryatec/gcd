namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IFeedGitHub
{
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderAddLocalPackage WithAddLocalPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderAddHttpPackage WithAddHttpPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderPullMetaData WithPullMetaDataCmd(); 
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedGitHub.ArgBuilderPushMetaData WithPushMetaDataCmd(); 
}