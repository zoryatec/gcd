using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageXbUserVisible
    {
        public static PackageXbUserVisible Default => new PackageXbUserVisible("yes");
        public static Result<PackageXbUserVisible> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageXbUserVisible(value));
        }
        private PackageXbUserVisible(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-UserVisible";
        public override string ToString() => Value;
    }
}
