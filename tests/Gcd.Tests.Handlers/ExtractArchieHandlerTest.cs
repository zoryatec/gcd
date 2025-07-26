using System.IO.Compression;
using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.Services;
using Gcd.Tests.Fixture;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Gcd.Tests.Handlers;

public class ExtractArchieHandlerTest(TestFixture testFixture) : IClassFixture<TestFixture>
{
    [Fact]
    public async Task Test1()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var webDownload = new Mock<IWebDownload>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        var fileSystem = new LocalFileService();
        var handler = new ExtractArchiveHandler(fileSystem);

        var tempDirectoryToZipResult = await fileSystem.GenerateTempDirectoryAsync();
        var tempDirectoryToZip = tempDirectoryToZipResult.Value;

        var tempDirectoryToPutZipFileResult = await fileSystem.GenerateTempDirectoryAsync();
        var tempDirectoryToPutZipFile = tempDirectoryToPutZipFileResult.Value;
        var zipFilePath = tempDirectoryToPutZipFile.Value + "\\test.zip";

        var contentDirPath = tempDirectoryToZip.Value + "\\content";
        var testFilePath = contentDirPath + "\\sample.txt";
        var testValue = Guid.NewGuid().ToString();
        Directory.CreateDirectory(contentDirPath);
        File.WriteAllText(testFilePath, testValue);


        ZipFile.CreateFromDirectory(tempDirectoryToZip.Value, zipFilePath);
        

        var desinationDirecotyr = await fileSystem.GenerateTempDirectoryAsync();
        var relativeContendDirPath = RelativeDirPath.Of("content");
        var request = new ExtractArchiveRequest(
            LocalFilePath.Of(zipFilePath).Value,
            relativeContendDirPath.Value,
            desinationDirecotyr.Value);

        var result = await handler.Handle(request, CancellationToken.None);

        var resultTestFilePath = desinationDirecotyr.Value + "\\sample.txt";
        var resultTetFile = LocalFilePath.Of(resultTestFilePath).Value;
        var testResult = await fileSystem.ReadTextFileAsync(resultTetFile, CancellationToken.None);

        Assert.Equal(testValue, testResult);
    }
}