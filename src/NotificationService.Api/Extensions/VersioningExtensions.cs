using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring API versioning in the application.
/// </summary>
public static class VersioningExtensions
{
    /// <summary>
    /// Configures API versioning services for the application.
    /// This method sets the default API version, ensures the default version is assumed when not specified,
    /// and enables reporting of API versions.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the versioning services to.</param>
    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
    }
}