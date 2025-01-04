using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageXBSection : ControlFileProperty
    {
        public static PackageXBSection Default => new PackageXBSection("unset-section");
        public static Result<PackageXBSection> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageXBSection(value));
        }
        private PackageXBSection(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-Section";
        public override string ToString() => Value;
    }
}
