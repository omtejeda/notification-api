namespace NotificationService.Application.Common.Dtos;

public class TemplateLabelDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? CatalogNameToCheckAgainst { get; set; }
}