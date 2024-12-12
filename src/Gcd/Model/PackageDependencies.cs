using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record PackageDependencies : ControlFileProperty
{
    public static PackageDependencies Default => new PackageDependencies("");
    public static Result<PackageDependencies> Of(string value)
    {
        return Result.Success()
            .Map(() => new PackageDependencies(value));
    }
    private PackageDependencies(string value) => Value = value;
    public string Value { get; }
    public static string Key { get; } = "Depends";
    public override string ToString() => Value;
}

