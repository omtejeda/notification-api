namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Provides date and time services, including current local and UTC times.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current date and time in Coordinated Universal Time (UTC).
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Converts the current UTC time to local time.
    /// </summary>
    DateTime UtcToLocalTime { get; }
}