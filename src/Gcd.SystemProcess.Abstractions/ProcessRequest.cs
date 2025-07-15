namespace Gcd.SystemProcess.Abstractions;

public record ProcessRequest(string ExecutablePath, string[] Arguments);