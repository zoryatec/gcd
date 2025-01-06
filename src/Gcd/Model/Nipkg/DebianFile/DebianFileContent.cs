namespace Gcd.Model.Nipkg.DebianFile;

public record DebianFileContent
{
    public static DebianFileContent Default => new DebianFileContent("2.0");
    private DebianFileContent(string value) => Value = value;
    public string Value { get; }
    public override string ToString() => Value;
}

