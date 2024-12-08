using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgDownloadNipkg;

public record DownloadNipkgRequest(FilePath FilePath) : IRequest<Result>;


public class DownloadNipkgHandler(IWebDownload _webDownload)
    : IRequestHandler<DownloadNipkgRequest, Result>
{
    public async Task<Result> Handle(DownloadNipkgRequest request, CancellationToken cancellationToken)
    {
        var nipkgInstaller = "NIPackageManager21.3.0_online.exe";
        var url = $"https://download.ni.com/support/nipkg/products/ni-package-manager/installers/{nipkgInstaller}";

        var webUri = WebUri.Create(url);

        if (File.Exists(request.FilePath.Value)) File.Delete(request.FilePath.Value);

        return await _webDownload.DownloadFileAsync(webUri.Value, request.FilePath);
    }
}



