using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.Common
{
    public record PackageHttpPath : WebFileUri, IPackageFileDescriptor
    {
        public static Result<PackageHttpPath> Of(Maybe<string> maybeValue)
        {
            return maybeValue.ToResult($"{nameof(WebUri)} should not be empty")
            .Ensure(arg => arg != string.Empty, "{nameof(WebUri)}  should not be empty")
            .MapTry((arg) => new Uri(arg), ex => ex.Message)
            .Map(arg => new PackageHttpPath(arg));
        }

        private PackageHttpPath(Uri uri) : base(uri)
        {
            var test = uri.AbsolutePath;
            var file = Path.GetFileName(uri.AbsolutePath);
            if (string.IsNullOrEmpty(file)) throw new NullReferenceException("no file ");
            var packageFileRes = PackageFileName.Of(file);
            FileName = packageFileRes.Value;
        }

        public static PackageLocalFilePath Of(LocalDirPath Directory, PackageFileName FileName)
        {
            return new PackageLocalFilePath(Directory, FileName);
        }

        public PackageFileName FileName { get; }

    }
}
