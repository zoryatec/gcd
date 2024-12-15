using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public record PackageFileName(PackageArchitecture Architecture, PackageName Name, PackageVersion Version)
    {
        public string Value { get => $"{Name.Value}_{Version.Value}_{Architecture.Value}.nipkg"; }
    }
}
