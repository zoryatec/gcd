using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageXbUserVisible : ControlFileProperty
    {
        public static PackageXbUserVisible Default => new PackageXbUserVisible("yes");
        public static Result<PackageXbUserVisible> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageXbUserVisible(value));
        }
        private PackageXbUserVisible(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-UserVisible";
        public override string ToString() => Value;
    }
}
