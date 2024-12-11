namespace Gcd.Model
{
    public record PackageXBSection
    {
        public static PackageXBSection Default => new PackageXBSection("unset-section");
        private PackageXBSection(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
