using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public interface IPackageFileDescriptor : IFileDescriptor
    {
        public PackageFileName FileName { get; }
    }
}
