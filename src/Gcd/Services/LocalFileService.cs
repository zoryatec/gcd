using CSharpFunctionalExtensions;
using Gcd.Model;


namespace Gcd.Services
{
    public class LocalFileService : IFileSystem
    {
        public async Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default) =>
            ReadTextFile(filePath);

        private Result<string> ReadTextFile(LocalFilePath filePath) =>
            Result.Try(() => File.ReadAllText(filePath.Value), ex => ex.Message);

        public async Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default) =>
            WriteTextFile(filePath, content);

        private Result WriteTextFile(LocalFilePath filePath, string content) =>
            Result.Try(() => File.WriteAllText(filePath.Value, content), ex => ex.Message);

        public async Task<Result> CreateDirectoryAsync(LocalDirPath path) =>
            Result.Try(() => Directory.CreateDirectory(path.Value));


        public async Task<Result<bool>> CheckDirectoryExists(LocalDirPath path) =>
             Result.Try(() => Directory.Exists(path.Value), ex => ex.Message);


        public async Task<Result> CopyFileAsync(LocalFilePath source, LocalFilePath destination, bool overwrite = false, CancellationToken cancellationToken = default) =>
            Result.Try(() => File.Copy(source.Value, destination.Value, overwrite: overwrite));

        public async Task<Result<LocalDirPath>> CreateTempDirPathAsync()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            return LocalDirPath.Parse(path);
        }

        public string GenerateTempDirectory()
        {
            string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            if (Directory.Exists(temporaryDirectory))
                Directory.Delete(temporaryDirectory, true);

            Directory.CreateDirectory(temporaryDirectory);
            return temporaryDirectory;
        }

        public async Task<Result<LocalDirPath>> GenerateTempDirectoryAsync()
        {
            try
            {
                var path = GenerateTempDirectory();
                return LocalDirPath.Parse(path);
            }
            catch (Exception ex)
            {
                return Result.Failure<LocalDirPath>(ex.Message);
            }
        }
    }


}
