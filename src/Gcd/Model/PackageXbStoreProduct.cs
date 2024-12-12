using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageXbStoreProduct : ControlFileProperty
    {
        public static PackageXbStoreProduct Default => new PackageXbStoreProduct("yes");
        public static Result<PackageXbStoreProduct> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageXbStoreProduct(value));
        }
        private PackageXbStoreProduct(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-StoreProduct";
        public override string ToString() => Value;
    }
}

