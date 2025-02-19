using FluentAssertions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Tests.LocalFileSystem;

public class LocalDirectoryPathTests
{
    [Fact]
    public void Of_ValidInput_ReturnsDirectoryPath()
    {
        // Arragne
        var inputPath = @"C:\test\myDir";
        
        // Act
        var result = LocalDirPath.Of(inputPath);
        
        // Assert
        result.Should().Succeed();
    }
    
    [Fact]
    public void Of_NullInput_ReturnsErrorNullValue()
    {
        // Arrange
        string inputPath = null;
        
        // Act
        var result = LocalDirPath.Of(inputPath);
        
        // Assert
        result.Should().FailWith(ErorNullValue.Of(nameof(LocalDirPath)));
    }
    
    [Fact]
    public void Of_EmptyValue_ReturnsErrorErrorEmptyValue()
    {
        // Arrange
        string inputPath = "";
        
        // Act
        var result = LocalDirPath.Of(inputPath);
        
        // Assert
        result.Should().FailWith(ErrorEmptyValue.Of(nameof(LocalDirPath)));
    }
    [Fact]
    public void Of_RelativePathValue_ReturnsDirectoryPath()
    {
        // Arrange
        string inputPath = "relativeDir";
        string expectedPath = Path.Combine(Environment.CurrentDirectory, inputPath);
        // Act
        var result = LocalDirPath.Of(inputPath);
        
        // Assert
        result.Should().Succeed();
        result.Value.Value.Should().Be(expectedPath);
    }
}

