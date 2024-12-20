using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Model.File;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.Tools.DownloadNipkg;

public record DownloadNipkgRequest(LocalFilePath FilePath, NipkgInstallerUri InstallerUri) : IRequest<Result>;

public class DownloadNipkgHandler(IWebDownload _webDownload, NipkgInstallerUri _installerUri)
    : IRequestHandler<DownloadNipkgRequest, Result>
{
    public async Task<Result> Handle(DownloadNipkgRequest request, CancellationToken cancellationToken)
    {
        return await WebUri.Create(_installerUri.Value)
            .Bind(uri => DownloadFileAsync(uri, request.FilePath));
    }

    public async Task<Result> DownloadFileAsync(WebUri webUri, LocalFilePath filePath)
    {
        if (File.Exists(filePath.Value)) File.Delete(filePath.Value);
        return await _webDownload.DownloadFileAsync(webUri, filePath);
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> DownloadNipkgInstallerAsync(this IMediator mediator, LocalFilePath FilePath, NipkgInstallerUri InstallerUri, CancellationToken cancellationToken = default)
        => await mediator.Send(new DownloadNipkgRequest(FilePath, InstallerUri), cancellationToken);
}

