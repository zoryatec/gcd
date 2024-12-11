namespace Gcd.Model
{
    public record PackageXbPlugin
    {
        public static PackageXbPlugin Default => new PackageXbPlugin("file");
        private PackageXbPlugin(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
