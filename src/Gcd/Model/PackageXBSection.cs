using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageXBSection
    {
        public static PackageXBSection Default => new PackageXBSection("unset-section");
        public static Result<PackageXBSection> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageXBSection(value));
        }
        private PackageXBSection(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-Section";
        public override string ToString() => Value;
    }
}
