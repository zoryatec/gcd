using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageXbPlugin
    {
        public static PackageXbPlugin Default => new PackageXbPlugin("file");
        public static Result<PackageXbPlugin> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageXbPlugin(value));
        }
        private PackageXbPlugin(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-Plugin";
        public override string ToString() => Value;
    }
}


