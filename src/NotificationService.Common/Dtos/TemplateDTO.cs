using NotificationService.Domain.Models;

namespace NotificationService.Common.Dtos;

public class TemplateDto
{
    public string TemplateId { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Language { get; set; }
    public string NotificationType { get; set; }
    public string PlatformName { get; set; }
    public ICollection<Metadata> Metadata { get; set; }
    public ICollection<TemplateLabelDto> Labels { get; set; }
}