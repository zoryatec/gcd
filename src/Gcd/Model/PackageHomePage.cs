namespace Gcd.Model
{
    public record PackageHomePage
    {
        public static PackageHomePage Default => new PackageHomePage("unset-home-page");
        private PackageHomePage(string value) => Value = value;
        public string Value { get; }
        public override string ToString() => Value;
    }
}
