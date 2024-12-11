namespace Gcd.Model
{
    public record PackageMaintainer
    {
        public static PackageMaintainer Default => new PackageMaintainer("unset-maintainer");
        private PackageMaintainer(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
