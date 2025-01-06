using CSharpFunctionalExtensions;
using static System.Collections.Specialized.BitVector32;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageDescription : ControlFileProperty
    {
        public static PackageDescription Default => new PackageDescription("unset-description");
        public static Result<PackageDescription> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageDescription(value));
        }
        private PackageDescription(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Description";
        public override string ToString() => Value;
    }
}