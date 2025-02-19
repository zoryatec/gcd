using Gcd.Services.RemoteFileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.RemoteFileSystem.Git.DI;

public static class RemoteFileSystemGitExt
{

    public static IServiceCollection AddRemoteFileServiceGit(this IServiceCollection services) =>
        services.AddScoped<IRemoteFileSystemGit, RemoteFileSystemGit>();
}

