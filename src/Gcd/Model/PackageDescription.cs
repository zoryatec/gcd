using CSharpFunctionalExtensions;
using static System.Collections.Specialized.BitVector32;

namespace Gcd.Model
{
    public record PackageDescription : ControlFileProperty
    {
        public static PackageDescription Default => new PackageDescription("unset-description");
        public static Result<PackageDescription> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageDescription(value));
        }
        private PackageDescription(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Description";
        public override string ToString() => Value;
    }
}