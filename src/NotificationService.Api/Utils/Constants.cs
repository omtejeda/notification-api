namespace NotificationService.Api.Utils;

/// <summary>
/// Provides API versioning constants for the application.
/// </summary>
public static class ApiVersions
{
    /// <summary>
    /// The first version of the API.
    /// </summary>
    public const string v1 = "1.0";

    /// <summary>
    /// The second version of the API.
    /// </summary>
    public const string v2 = "2.0";
}

/// <summary>
/// Contains route constants for the API.
/// </summary>
public static class Routes
{
    /// <summary>
    /// The global prefix for API routes, including versioning.
    /// </summary>
    public const string GlobalPrefix = "api/v{version:apiVersion}/notification";

    /// <summary>
    /// The route template for the controller route.
    /// </summary>
    public const string ControllerRoute = "[controller]";
}