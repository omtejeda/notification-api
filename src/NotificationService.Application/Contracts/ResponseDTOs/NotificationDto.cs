namespace NotificationService.Application.Contracts.ResponseDtos;

public class NotificationDto
{
    public string? NotificationId { get; set; }
    public string? Type { get; set; }
    public string? ToDestination { get; set; }
    public string? TemplateName { get; set; }
    public string? PlatformName { get; set; }
    public string? ProviderName { get; set; }
    public DateTime? Date { get; set; }
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public bool HasAttachments { get; set; }
    public string? ParentNotificationId { get; set; }
    public string? Subject { get; set; }
    public string? From { get; set; }
    public ICollection<AttachmentDto> Attachments { get; set; } = [];
}