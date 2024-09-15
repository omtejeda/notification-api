namespace NotificationService.Application.Features.Templates.Models;

public record SendGridTemplate
{
    public bool HasTemplate { get; set; } = false;
    public string? TemplateId { get; set; }
    public string? Category { get; set; }
    public object? DynamicTemplateData { get; set; }
}