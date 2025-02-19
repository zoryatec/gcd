using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;

namespace Gcd.RemoteFileSystem.AzBlob.Abstractions;



public record AzBlobSas
{
    public static Result<AzBlobSas> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("FeedUri should not be empty")
            .Ensure(value => value != string.Empty, "FeedUri should not be empty")
            .Map(value => new AzBlobSas(value));
    }
    private AzBlobSas(string value) => Value = value;
    public string Value;
}


