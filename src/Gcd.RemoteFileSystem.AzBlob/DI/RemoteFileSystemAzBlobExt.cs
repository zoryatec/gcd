using Gcd.Services;
using Gcd.Services.RemoteFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.AzBlob.DI;

public static class RemoteFileSystemAzBlobExt
{

    public static IServiceCollection AddRemoteFileServiceAzBlob(this IServiceCollection services) =>
        services.AddScoped<IRemoteFileSystemAzBlob, RemoteFileSystemAzBlob>()
                .AddScoped<IDownloadAzBlobService, AzBlobService>()
                .AddScoped<IUploadAzBlobService, AzBlobService>()
                .AddScoped<IRemoteFileSystemAzBlob, RemoteFileSystemAzBlob>();
}

