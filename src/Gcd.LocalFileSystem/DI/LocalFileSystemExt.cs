using Gcd.LocalFileSystem;
using Gcd.LocalFileSystem.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.Git.DI;

public static class LocalFileSystemExt
{

    public static IServiceCollection AddLocalFileSystem(this IServiceCollection services) =>
        services.AddScoped<IFileSystem, LocalFileService>();
}

