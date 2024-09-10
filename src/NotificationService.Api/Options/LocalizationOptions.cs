namespace NotificationService.Api.Options;

public record LocalizationOptions
{
    public string DefaultCulture{ get; set; }
    public string[] SupportedCultures { get; set; }
}