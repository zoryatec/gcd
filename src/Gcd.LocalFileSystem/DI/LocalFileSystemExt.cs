using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.Git.DI;

public static class LocalFileSystemExt
{

    public static IServiceCollection AddLocalFileSystem(this IServiceCollection services) =>
        services.AddScoped<IFileSystem, LocalFileService>();
}

