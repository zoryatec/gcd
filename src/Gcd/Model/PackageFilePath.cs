using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public record PackageFilePath : LocalFilePath
    {
        public PackageFilePath(LocalDirPath directory, PackageFileName fileName) : base($"{directory.Value}\\{fileName.Value}")
        {
            Directory = directory;
            FileName = fileName;
        }

        public static PackageFilePath Of(LocalDirPath Directory, PackageFileName FileName)
        {
            return new PackageFilePath(Directory, FileName);
        }

        public LocalDirPath Directory { get; }
        public PackageFileName FileName { get; }
    }
}
