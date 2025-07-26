using System.Text;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.LocalFileSystem
{
    public class LocalFileService : IFileSystem
    {
        public async Task<Result> WriteAllLinesAsync(ILocalFilePath filePath, IEnumerable<string> content, Encoding encoding,
            CancellationToken cancellationToken = default) =>
            await Result.Try(() =>  File.WriteAllLinesAsync(filePath.Value, content, encoding, cancellationToken),
                ex => ex.Message);
        

        public async Task<Result<string>> ReadTextFileAsync(ILocalFilePath filePath, CancellationToken cancellationToken = default) =>
            ReadTextFile(filePath);

        private Result<string> ReadTextFile(ILocalFilePath filePath) =>
            Result.Try(() => File.ReadAllText(filePath.Value), ex => ex.Message);

        public Result<IReadOnlyList<ILocalFilePath>, Error> ListFiles(ILocalDirPath directoryPath, string searchPattern = "*.*", bool recursive = false)
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(directoryPath.Value, searchPattern, searchOption);
            var list  = new List<ILocalFilePath>();
            foreach (var file in files)
            {
                var result  = LocalFilePath.Of(file);
                if (result.IsFailure) return Result.Failure<IReadOnlyList<ILocalFilePath>,Error>(result.Error);
                list.Add(result.Value);

            }

            return Result.Success<IReadOnlyList<ILocalFilePath>, Error>(list.AsReadOnly());
        }

        public async Task<Result> WriteTextFileAsync(ILocalFilePath filePath, string content, CancellationToken cancellationToken = default) =>
            WriteTextFile(filePath, content);

        private Result WriteTextFile(ILocalFilePath filePath, string content) =>
            Result.Try(() => File.WriteAllText(filePath.Value, content), ex => ex.Message);

        public async Task<Result> CreateDirectoryAsync(LocalDirPath path) =>
            Result.Try(() => Directory.CreateDirectory(path.Value));


        public async Task<Result<bool>> CheckDirectoryExists(LocalDirPath path) =>
             Result.Try(() => Directory.Exists(path.Value), ex => ex.Message);


        public async Task<Result> CopyFileAsync(ILocalFilePath source, ILocalFilePath destination, bool overwrite = false, CancellationToken cancellationToken = default) =>
            Result.Try(() => File.Copy(source.Value, destination.Value, overwrite: overwrite));

        public async Task<Result<LocalDirPath>> CreateTempDirPathAsync()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            return LocalDirPath.Of(path).MapError(er => er.Message);
        }

        public async Task<Result> CreateDirAsync(LocalDirPath locDirPath)
        {
            return Result.Try(() => CreateDir(locDirPath));
        }

        private void CreateDir(LocalDirPath locDirPath)
        {
            if (!Directory.Exists(locDirPath.Value)) Directory.CreateDirectory(locDirPath.Value);
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
                return LocalDirPath.Of(path).MapError(er => er.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure<LocalDirPath>((ex.Message));
            }
        }


        public async Task<Result> CopyDirectoryRecursievely(ILocalDirPath source, ILocalDirPath destination, bool overwrite = false, CancellationToken cancellationToken = default)
            => Result.Try(() => CopyDirectoryContents(source.Value, destination.Value), ex => ex.Message);
        private void CopyDirectoryContents(string sourceDir, string destinationDir)
        {
            // Ensure the source directory exists
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDir}");
            }

            // Create the destination directory if it does not exist
            Directory.CreateDirectory(destinationDir);

            // Copy all files from source to destination
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, overwrite: true);
            }

            // Recursively copy all subdirectories
            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string directoryName = Path.GetFileName(directory);
                string destDir = Path.Combine(destinationDir, directoryName);
                CopyDirectoryContents(directory, destDir);
            }
        }
    }

}
