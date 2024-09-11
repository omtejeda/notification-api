using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NotificationService.Api.Options;

namespace NotificationService.Api.Extensions;

public static class LocalizationExtensions
{
    private const string LocalizationSectionName = "Localization";

    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app, IConfiguration configuration)
    {
        var requestLocalizationOptions = GetRequestLocalizationOptions(configuration);
        app.UseRequestLocalization(requestLocalizationOptions);
        
        return app;
    }

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