namespace NotificationService.Common.Interfaces;

public interface IEnvironmentService
{
    string? CurrentEnvironment { get; }
    bool IsProduction { get; }
    int? GmtOffset { get; }
}