using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Gcd.Model.Feed;

public record FeedItem(
                PackageArchitecture Architecture,
                PackageHomePage HomePage,
                PackageMaintainer Maintainer,
                PackageDescription Description,
                PackageXbPlugin XbPlugin,
                PackageXbUserVisible XbUserVisible,
                PackageXbStoreProduct XbStoreProduct,
                PackageXBSection XBSection,
                PackageName Name,
                PackageVersion Version,
                PackageDependencies Dependencies)
{
    public static FeedItem Default =>
        new FeedItem(
                PackageArchitecture.Default,
                PackageHomePage.Default,
                PackageMaintainer.Default,
                PackageDescription.Default,
                PackageXbPlugin.Default,
                PackageXbUserVisible.Default,
                PackageXbStoreProduct.Default,
                PackageXBSection.Default,
                PackageName.Default,
                PackageVersion.Default,
                PackageDependencies.Default);


}


//Architecture: windows_x64
//CompatibilityVersion: 240800
//Depends: 
//Description: unset-description
//Filename: sample-package_99.88.77.66_windows_x64.nipkg
//Homepage: unset-home-page
//MD5sum: 299fa4854c696ab2559658c053e04e0f
//Maintainer: unset-maintainer
//Package: sample-package
//Plugin: file
//Section: unset-section
//Size: 768
//StoreProduct: yes
//UserVisible: yes
//Version: 99.88.77.66

