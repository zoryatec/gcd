using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageHomePage : ControlFileProperty
    {
        public static PackageHomePage Default => new PackageHomePage("unset-home-page");

        public static Result<PackageHomePage> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageHomePage(value));
        }

        private PackageHomePage(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Homepage";
        public override string ToString() => Value;
    }
}
