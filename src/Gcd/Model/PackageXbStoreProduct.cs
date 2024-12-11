namespace Gcd.Model
{
    public record PackageXbStoreProduct
    {
        public static PackageXbStoreProduct Default => new PackageXbStoreProduct("yes");
        private PackageXbStoreProduct(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
