using CSharpFunctionalExtensions;

namespace Gcd.Model.FeedDefinition;

public record GitLocalBranch
{
    public static Result<GitLocalBranch> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(GitLocalBranch)} cannot be empty")
            .Map(x => new GitLocalBranch(x));
    }
    private GitLocalBranch(string value) => Value = value;
    public string Value { get; }
}
