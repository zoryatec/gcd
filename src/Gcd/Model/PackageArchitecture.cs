using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageArchitecture
    {
        public static PackageArchitecture Default => new PackageArchitecture("windows_x64");
        
        public static Result<PackageArchitecture> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageArchitecture(value));
        }

        private PackageArchitecture(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Architecture";
        public override string ToString() => Value;
    }
}
