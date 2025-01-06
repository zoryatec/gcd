namespace Gcd.LocalFileSystem.Abstractions;

public  record FileName
{
    public FileName(string value) { Value = value; }

    public string Value { get; }
    public override string ToString() => Value;
}

