using CSharpFunctionalExtensions;

namespace Gcd.Model
{
    public record PackageHomePage : ControlFileProperty
    {
        public static PackageHomePage Default => new PackageHomePage("unset-home-page");

        public static Result<PackageHomePage> Of(string value)
        {
            return Result.Success()
                .Map(() => new PackageHomePage(value));
        }

        public static Result<PackageHomePage> From(Maybe<Dictionary<string, string>> maybeDict)
        {
            return maybeDict
                .ToResult("incorrect format of control file")
                .Ensure(dict => dict.ContainsKey(Key), "")
                .Bind(dict => PackageHomePage.Of(dict[Key]));
        }

        private PackageHomePage(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Homepage";
        public override string ToString() => Value;
    }
}
