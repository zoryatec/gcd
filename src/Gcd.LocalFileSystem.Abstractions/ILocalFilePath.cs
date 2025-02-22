namespace Gcd.LocalFileSystem.Abstractions;

public interface ILocalFilePath
{
    public string Value { get; }
    public ILocalDirPath Directory { get; }
    public IFileName FileName { get; }
}

