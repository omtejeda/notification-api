namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Provides information about the current environment in which the application is running.
/// </summary>
public interface IEnvironmentService
{
    /// <summary>
    /// Gets the name of the current environment (e.g., Development, Staging, Production).
    /// </summary>
    string? CurrentEnvironment { get; }

    /// <summary>
    /// Indicates whether the application is running in a production environment.
    /// </summary>
    bool IsProduction { get; }

    /// <summary>
    /// Gets the GMT offset of the current environment in hours.
    /// </summary>
    int? GmtOffset { get; }
}