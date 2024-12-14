using CSharpFunctionalExtensions;


namespace Gcd.Services
{
    public class LocalFileService : ITextFileReader, ITextFileWriter
    {
        public async Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default) =>
            ReadTextFile(filePath);

        private Result<string> ReadTextFile(LocalFilePath filePath) =>
            Result.Try(() => File.ReadAllText(filePath.Value), ex => ex.Message);

        public async Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default) =>
            WriteTextFile(filePath, content);

        private Result WriteTextFile(LocalFilePath filePath, string content) =>
            Result.Try(() => File.WriteAllText(filePath.Value, content), ex => ex.Message);
    }


}
