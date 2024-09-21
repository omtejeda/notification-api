using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.SharedKernel;

namespace NotificationService.Domain.Entities;

public class Template : EntityBase
{
    public string TemplateId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public Language Language { get; set; }
    public NotificationType NotificationType { get; set; }
    public string PlatformName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<Metadata> Metadata { get; set; } = [];
    public ICollection<TemplateLabel> Labels { get; set; } = [];
}

public class TemplateLabel
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string CatalogNameToCheckAgainst { get; set; } = string.Empty;
}