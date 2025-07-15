namespace Gcd.SystemProcess.Abstractions;

public interface IProcessService
{
    public Task<ProcessResponse> ExecuteAsync(ProcessRequest request, CancellationToken cancellationToken = default);
    public Task<ProcessResponse> ExecuteAsync(string executablePath, string[] arguments, CancellationToken cancellationToken = default);
}