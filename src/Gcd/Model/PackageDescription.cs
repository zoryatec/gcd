namespace Gcd.Model
{
    public record PackageDescription
    {
        public static PackageDescription Default => new PackageDescription("unset-description");
        private PackageDescription(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
