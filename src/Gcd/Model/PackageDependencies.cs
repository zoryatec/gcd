namespace Gcd.Model
{
    public record PackageDependencies
    {
        public static PackageDependencies Default => new PackageDependencies("");
        private PackageDependencies(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
