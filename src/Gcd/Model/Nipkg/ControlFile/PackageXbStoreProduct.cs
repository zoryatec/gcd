using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageXbStoreProduct : ControlFileProperty
    {
        public static PackageXbStoreProduct Default => new PackageXbStoreProduct("yes");
        public static Result<PackageXbStoreProduct> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageXbStoreProduct(value));
        }
        private PackageXbStoreProduct(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-StoreProduct";
        public override string ToString() => Value;
    }
}

