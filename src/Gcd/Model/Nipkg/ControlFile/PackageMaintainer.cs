using CSharpFunctionalExtensions;

namespace Gcd.Model.Nipkg.ControlFile
{
    public record PackageMaintainer : ControlFileProperty
    {
        public static PackageMaintainer Default => new PackageMaintainer("unset-maintainer");

        public static Result<PackageMaintainer> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult("value cannot be empty")
                .Map((value) => new PackageMaintainer(value));
        }
        private PackageMaintainer(string value) => Value = value;
        public string Value { get; }
        public static string Key { get; } = "Maintainer";
        public override string ToString() => Value;
    }
}
