namespace Gcd.SystemProcess.Abstractions;

public record ProcessResponse(int ExitCode, string StandardOutput, string StandardError);