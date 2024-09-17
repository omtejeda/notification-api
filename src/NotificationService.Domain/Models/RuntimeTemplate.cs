using NotificationService.Domain.Dtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public class RuntimeTemplate
{
    public string Name { get; set; } = string.Empty;
    public string PlatformName { get; set; } = string.Empty;
    public Language Language { get; set; }
    public List<MetadataDto> ProvidedMetadata { get; set; } = [];
    public string Content { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}