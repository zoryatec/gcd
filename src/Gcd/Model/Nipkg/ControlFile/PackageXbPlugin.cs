using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageXbPlugin : ControlFileProperty
    {
        public static PackageXbPlugin Default => new PackageXbPlugin("file");
        public static Result<PackageXbPlugin> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageXbPlugin(value));
        }
        private PackageXbPlugin(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "XB-Plugin";
        public override string ToString() => Value;
    }
}


