using Gcd.Services.RemoteFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.Smb.DI;

public static class RemoteFileSystemSmbExt
{

    public static IServiceCollection AddRemoteFileServiceSmb(this IServiceCollection services) =>
        services.AddScoped<IRemoteFileSystemSmb, RemoteFileSystemSmb>();
}

