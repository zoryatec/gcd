using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model;
using Gcd.Services;
using MediatR;


namespace Gcd.Commands.NipkgPushAzBlobFeedMeta;

public record NipkgPushAzBlobFeedMetaRequest(AzBlobFeedDefinition AzFeedDefinition, LocalFeedDefinition LocalFeedDefinition) : IRequest<Result>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler(IUploadAzBlobService uploadService)
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }

    private async Task<Result> UploadFileAsync(AzBlobUri uri, LocalFilePath filePath) =>
     await uploadService.UploadFileAsync(uri, filePath);
}

