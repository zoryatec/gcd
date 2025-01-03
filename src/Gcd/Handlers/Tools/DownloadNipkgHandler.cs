using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Model.File;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Tools;

public record DownloadNipkgRequest(LocalFilePath FilePath, NipkgInstallerUri InstallerUri) : IRequest<Result>;

public class DownloadNipkgHandler(IWebDownload _webDownload, NipkgInstallerUri _installerUri)
    : IRequestHandler<DownloadNipkgRequest, Result>
{
    public async Task<Result> Handle(DownloadNipkgRequest request, CancellationToken cancellationToken)
    {
        var uri = request.InstallerUri;
        if (uri == NipkgInstallerUri.None)
        {
            uri = _installerUri;
            if (uri == NipkgInstallerUri.None) return Result.Failure("Please specify NIPKG uri");
        }

        return await WebUri.Create(_installerUri.Value)
            .Bind(uri => DownloadFileAsync(uri, request.FilePath));
    }

    public async Task<Result> DownloadFileAsync(WebUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await _webDownload.DownloadFileAsync(webUri, filePath);
    }
}


