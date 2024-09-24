namespace NotificationService.Api.Options;

/// <summary>
/// Represents the localization options for the application.
/// This record is used to capture the Localization section in the appsettings.json file.
/// </summary>
/// <param name="DefaultCulture">The default culture for localization.</param>
/// <param name="SupportedCultures">An array of supported cultures for localization.</param>
public record LocalizationOptions(string DefaultCulture, string[] SupportedCultures);