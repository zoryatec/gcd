using FluentAssertions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Tests.LocalFileSystem;

public class LocalFilePathTests
{
    [Fact]
    public void Of_ValidInput_ReturnsFilePath()
    {
        // Arragne
        var validPath = @"C:\test\myFile.txt";
        
        // Act
        var result = LocalFilePath.Of(validPath);
        
        // Assert
        result.Should().Succeed();
    }
    
    [Fact]
    public void Of_NullInput_ReturnsErrorNullValue()
    {
        // Arrange
        string inputPath = null;
        
        // Act
        var result = LocalFilePath.Of(inputPath);
        
        // Assert
        result.Should().FailWith(ErorNullValue.Of(nameof(LocalFilePath)));
    }
    
    [Fact]
    public void Of_EmptyValue_ReturnsErrorErrorEmptyValue()
    {
        // Arrange
        string inputPath = "";
        
        // Act
        var result = LocalFilePath.Of(inputPath);
        
        // Assert
        result.Should().FailWith(ErrorEmptyValue.Of(nameof(LocalFilePath)));
    }
}

