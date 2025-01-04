using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageArchitecture : ControlFileProperty
    {
        public static PackageArchitecture Default => new PackageArchitecture("windows_x64");

        public static Result<PackageArchitecture> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageArchitecture(value));
        }

        private PackageArchitecture(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Architecture";
        public override string ToString() => Value;
    }
}
