using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Tools;

public record DownloadNipkgRequest(LocalFilePath FilePath, NipkgInstallerUri InstallerUri) : IRequest<UnitResult<Error>>;

public class DownloadNipkgHandler(IWebDownload _webDownload)
    : IRequestHandler<DownloadNipkgRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(DownloadNipkgRequest request, CancellationToken cancellationToken)
    {
        var(filePath, installerSourceUri) = request;
        //var uri = request.InstallerUri;
        // if (uri == NipkgInstallerUri.None)
        // {
        //     uri = _installerUri;
        //     if (uri == NipkgInstallerUri.None) return UnitResult.Failure(new Error("Please specify NIPKG uri") );
        // }

        return await WebUri.Create(installerSourceUri.Value)
            .Bind(uri => DownloadFileAsync(uri, filePath))
            .MapError(x => new Error(x));
    }

    public async Task<Result> DownloadFileAsync(WebUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await _webDownload.DownloadFileAsync(webUri, filePath);
    }
}


