using Gcd.RemoteFileSystem.Rclone.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.Rclone.DI;

public static class RemoteFileSystemRcloneExt
{

    public static IServiceCollection AddRemoteFileServiceRclone(this IServiceCollection services) =>
        services.AddScoped<IRemoteFileSystemRclone, RemoteFileSystemRclone>();
}

