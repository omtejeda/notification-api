using NotificationService.Api.Options;

namespace NotificationService.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring localization in the application.
/// </summary>
public static class LocalizationExtensions
{
    private const string LocalizationSectionName = "Localization";

    /// <summary>
    /// Configures the application to use localization based on the specified configuration settings.
    /// This method sets up the request localization options for the application.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance to configure.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing localization settings.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance, allowing for method chaining.</returns>
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app, IConfiguration configuration)
    {
        var requestLocalizationOptions = GetRequestLocalizationOptions(configuration);
        app.UseRequestLocalization(requestLocalizationOptions);
        
        return app;
    }

    /// <summary>
    /// Retrieves the <see cref="RequestLocalizationOptions"/> based on the provided configuration.
    /// This method extracts localization options from the configuration section and prepares
    /// them for use in the application.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing localization settings.</param>
    /// <returns>A <see cref="RequestLocalizationOptions"/> object configured with the specified options.</returns>
    public static RequestLocalizationOptions GetRequestLocalizationOptions(IConfiguration configuration)
    {
        LocalizationOptions options = configuration
            .GetSection(LocalizationSectionName)
            .Get<LocalizationOptions>();
        
        return new RequestLocalizationOptions()
            .SetDefaultCulture(options.DefaultCulture)
            .AddSupportedCultures(options.SupportedCultures)
            .AddSupportedUICultures(options.SupportedCultures);
    }
}