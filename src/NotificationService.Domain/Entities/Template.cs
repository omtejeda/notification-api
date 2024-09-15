using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Domain.Entities;

public class Template : BaseEntity
{
    public string TemplateId { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public Language Language { get; set; }
    public NotificationType NotificationType { get; set; }
    public string PlatformName { get; set; }
    public string Content { get; set; }
    public string Location { get; set; }
    public ICollection<Metadata> Metadata { get; set; }
    public ICollection<TemplateLabel> Labels { get; set; }
}

public class TemplateLabel
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string CatalogNameToCheckAgainst { get; set; }
}