using NotificationService.SharedKernel.Interfaces;
using NotificationService.SharedKernel.Constants;

namespace NotificationService.Application.Common.Services;

public class EnvironmentService : IEnvironmentService
{
    private const string EnvironmentKey = "ASPNETCORE_ENVIRONMENT";
    private const string GmtTimezoneKey = "GMT_TIMEZONE";

    public string? CurrentEnvironment
        => Environment.GetEnvironmentVariable(EnvironmentKey)?.ToString();
    
    public bool IsProduction
        => string.Equals(
            CurrentEnvironment,
            EnvironmentConstants.ProductionName,
            StringComparison.OrdinalIgnoreCase);
    
    public int? GmtOffset
        => GetIntEnvironmentVariable(GmtTimezoneKey);
    
    private static int? GetIntEnvironmentVariable(string name)
    {
        if (int.TryParse(Environment.GetEnvironmentVariable(name), out var value))
            return value;

        return default;
    }
}