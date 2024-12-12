using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageMaintainer
    {
        public static PackageMaintainer Default => new PackageMaintainer("unset-maintainer");

        public static Result<PackageMaintainer> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageMaintainer(value));
        }
        private PackageMaintainer(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Maintainer";
        public override string ToString() => Value;
    }
}
