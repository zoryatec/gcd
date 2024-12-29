using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public record PackageFilePath : LocalFilePath, IPackageFileDescriptor
    {
        public PackageFilePath(LocalDirPath directory, PackageFileName fileName) : base($"{directory.Value}\\{fileName.Value}")
        {
            Directory = directory;
            FileName = fileName;
        }

        public static Result<PackageFilePath> Of(Maybe<string> packagePathOrNothing)
        {
            var pkgName = Path.GetFileName(packagePathOrNothing.Value);
            var dir = Path.GetDirectoryName(packagePathOrNothing.Value);
            var locDir = LocalDirPath.Parse(dir); ;

            var packageFileName = PackageFileName.Of(pkgName);
            return packagePathOrNothing.ToResult("FeedUri should not be empty")
                .Ensure(packagePath => packagePath != string.Empty, "Package path should not be empty")
                .Map(feedUri => new PackageFilePath(locDir.Value, packageFileName.Value));
        }

        public static PackageFilePath Of(LocalDirPath Directory, PackageFileName FileName)
        {
            return new PackageFilePath(Directory, FileName);
        }

        public LocalDirPath Directory { get; }
        public PackageFileName FileName { get; }
    }
}
