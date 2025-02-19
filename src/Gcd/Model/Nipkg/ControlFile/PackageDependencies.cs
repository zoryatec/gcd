using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile;

public record PackageDependencies : ControlFileProperty
{
    public static PackageDependencies Default => new PackageDependencies("");
    public static Result<PackageDependencies> Of(Maybe<string> maybeValue)
    {
        return maybeValue.ToResult("value cannot be empty")
            .Map((value) => new PackageDependencies(value));
    }
    private PackageDependencies(string value) => Value = value;
    public string Value { get; }
    public static string Key { get; } = "Depends";
    public override string ToString() => Value;
}

