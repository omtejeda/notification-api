namespace NotificationService.Core.Templates.Models;

public record SendgridTemplate
{
    public bool HasTemplate { get; set; } = false;
    public string TemplateId { get; set; }
    public string Category { get; set; }
    public object DynamicTemplateData { get; set; }
}