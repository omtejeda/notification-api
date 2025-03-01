/// <summary>
/// Represents metadata information with key-value pairs.
/// </summary>
namespace NotificationService.Application.Features.Providers.Libraries.JsonParser;
public class Metadata
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}