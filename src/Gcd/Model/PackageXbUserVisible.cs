namespace Gcd.Model
{
    public record PackageXbUserVisible
    {
        public static PackageXbUserVisible Default => new PackageXbUserVisible("yes");
        private PackageXbUserVisible(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
