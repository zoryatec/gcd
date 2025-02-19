using FluentAssertions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.LocalFileSystem.Abstractions.Errors;

namespace Gcd.Tests.LocalFileSystem;

public class FileNameTests
{
    [Fact]
    public void Of_ValidInput_ReturnsFilePath()
    {
        // Arragne
        var input = @"myFile.txt"; // this is reminder to handle that properly
        
        // Act
        var result = FileName.Of(input);
        
        // Assert
        result.Should().Succeed();
        result.Value.Extension.Should().Be(FileExtension.Txt);
    }
    
    [Fact]
    public void Of_NullInput_ReturnsErrorNullValue()
    {
        // Arrange
        string input = null;
        
        // Act
        var result = FileName.Of(input);
        
        // Assert
        result.Should().FailWith(ErorNullValue.Of(nameof(FileName)));
    }
    
    [Fact]
    public void Of_EmptyValue_ReturnsErrorErrorEmptyValue()
    {
        // Arrange
        string input = "";
        
        // Act
        var result = FileName.Of(input);
        
        // Assert
        result.Should().FailWith(ErrorEmptyValue.Of(nameof(FileName)));
    }
    
    [Fact]
    public void Of_FullPathInput_ReturnsInvalidFileNameErrorh()
    {
        // Arrange
        var input = @"C:\myFile.txt"; // this is reminder to handle that properly
        
        // Act
        var result = FileName.Of(input);
        
        // Assert
        result.Should().FailWith(ErrorInvalidFileName.Of(input));
    }
}

