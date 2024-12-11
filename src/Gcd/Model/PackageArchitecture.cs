namespace Gcd.Model
{
    public record PackageArchitecture
    {
        public static PackageArchitecture Default => new PackageArchitecture("windows_x64");
        private PackageArchitecture(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
