﻿using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System.IO;


namespace Gcd.Services.FileSystem
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
                return LocalDirPath.Parse(path);
            }
            catch (Exception ex)
            {
                return Result.Failure<LocalDirPath>(ex.Message);
            }
        }


        public async Task<Result> CopyDirectoryRecursievely(LocalDirPath source, LocalDirPath destination, bool overwrite = false, CancellationToken cancellationToken = default)
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
