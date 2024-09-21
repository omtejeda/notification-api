using NotificationService.Domain.Models;

namespace NotificationService.Application.Common.Dtos;

public class TemplateDto
{
    public string TemplateId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty;
    public string PlatformName { get; set; } = string.Empty;
    public ICollection<Metadata> Metadata { get; set; } = [];
    public ICollection<TemplateLabelDto> Labels { get; set; } = [];
}