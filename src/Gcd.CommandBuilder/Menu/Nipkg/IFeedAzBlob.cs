namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IFeedAzBlob
{
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderAddLocalPackage WithAddLocalPackageCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderPullMetaData WithPullMetaDataCmd(); 
    public global::Gcd.CommandBuilder.Command.Nipkg.FeedAzBlob.ArgBuilderPushMetaData WithPushMetaDataCmd(); 
}