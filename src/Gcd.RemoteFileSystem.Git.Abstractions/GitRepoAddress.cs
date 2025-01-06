using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitRepoAddress
{
    public static Result<GitRepoAddress> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitRepoAddress)} cannot be empty")
            .Map(x => new GitRepoAddress(x));
    }
    private GitRepoAddress(string value) => Value = value;
    public string Value { get; }
}
