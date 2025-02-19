namespace Gcd.LocalFileSystem.Abstractions;

public interface IFileName
{
    public string Name { get; }
    public FileExtension Extension { get; }
    public string Value { get; }
}