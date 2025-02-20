namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IFeedSmb
{
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderAddLocalPackage WithAddLocalPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderPullMetaData WithPullMetaDataCmd(); 
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderPushMetaData WithPushMetaDataCmd(); 
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedSmb.ArgBuilderAddLocalDirectory WithAddLocalDirectoryCmd();
}