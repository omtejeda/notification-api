namespace NotificationService.SharedKernel.Utils;

/// <summary>
/// Provides utility methods for the application, including methods to handle environment variables and date/time calculations.
/// </summary>
public static class AppUtil
{
    /// <summary>
    /// Gets the GMT offset from the environment variable "GMT_TIMEZONE".
    /// </summary>
    public static int? Gmt
        => GetIntEnvironmentVariable("GMT_TIMEZONE");
    
    /// <summary>
    /// Gets the current date and time adjusted by the GMT offset.
    /// </summary>
    public static DateTime CurrentDate
        => DateTime.Now.AddHours(Gmt.GetValueOrDefault());

    /// <summary>
    /// Retrieves an integer value from an environment variable.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns>The integer value of the environment variable, or zero if the variable is not set or cannot be parsed.</returns>
    private static int GetIntEnvironmentVariable(string name)
    {
        return int.TryParse(Environment.GetEnvironmentVariable(name), out var value)
            ? value
            : default;
    }
}