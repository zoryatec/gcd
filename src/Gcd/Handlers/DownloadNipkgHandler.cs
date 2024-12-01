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
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record DownloadNipkgRequest(string DownloadDirectory) : IRequest<DownloadNipkgResponse>;
public record DownloadNipkgResponse(string Result);

public class DownloadNipkgHandler(IMediator _mediator)
    : IRequestHandler<DownloadNipkgRequest, DownloadNipkgResponse>
{
    public async Task<DownloadNipkgResponse> Handle(DownloadNipkgRequest request, CancellationToken cancellationToken)
    {
        var nipkgInstaller = "NIPackageManager21.3.0_online.exe";
        var url = $"https://download.ni.com/support/nipkg/products/ni-package-manager/installers/{nipkgInstaller}";
        var tempPath = $@"{request.DownloadDirectory}\nipkg-installer.exe";

        if (File.Exists(tempPath)) File.Delete(tempPath);

        DownloadNipkg(url, tempPath);


        return new DownloadNipkgResponse("result");
    }

    private void DownloadNipkg(string fileUrl, string downloadPath)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                // Download the file asynchronously
                client.DownloadFile(fileUrl, downloadPath);
                Console.WriteLine($"File downloaded successfully to {downloadPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
        }
    }

}



