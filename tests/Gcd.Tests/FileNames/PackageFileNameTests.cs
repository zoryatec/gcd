// using FluentAssertions;
// using Gcd.Common;
// using Gcd.LocalFileSystem.Abstractions;
// using Gcd.Model.Nipkg.Common;
//
// namespace Gcd.Tests.FileNames;
//
// public class PackageFileNameTests
// {
//     [Fact]
//     public void Of_NullInput_ReturnsErrorNullValue()
//     {
//         // Arrange
//         string test = null;
//             
//         // Act
//         var result = PackageFileName.Of(test);
//     
//         // Assert
//         result.Should().FailWith(ErorNullValue.Of(nameof(FileName)));
//     }
//
//     [Theory]
//     [InlineData("invalidPackageFileName.nipkg")]
//     [InlineData("invalidPackageFileName_test.nipkg")]
//     [InlineData("invalidPackage_566_test")]
//     public void Of_InvalidInput_ReturnsErrorNullValue(string input)
//     {
//         // Act
//         var result = PackageFileName.Of(input);
//
//         // Assert
//         result.Should().FailWith(ErrorInvalidPackageName.Of(input));
//     }
//
//     [Fact]
//     public void Of_ValidInput_ReturnsPackageFileName()
//     {
//         // Arrange
//         var architecture = "windows_x64";
//         var version = "99.88.77.66";
//         var name = "deb8acb7-c580-4bcb-bc1d-f6a2946d2966";
//         var extension = "nipkg";
//         string input = $"{name}_{version}_{architecture}.{extension}";
//             
//         // Act
//         var result = PackageFileName.Of(input);
//             
//         // 
//         result.Should().Succeed();
//         // result.Value.Architecture.Value.Should().Be(architecture);
//         // result.Value.Version.Value.Should().Be(version);
//         // result.Value.PackageName.Value.Should().Be(name);
//         result.Value.Extension.Value.Should().Be(extension);
//     }
// }