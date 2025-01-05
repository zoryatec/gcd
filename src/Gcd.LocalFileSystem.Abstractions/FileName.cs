namespace Gcd.LocalFileSystem.Abstractions;

public abstract record FileName
{
    protected FileName(string value) { Value = value; }

    public string Value { get; }
    public override string ToString() => Value;
}

