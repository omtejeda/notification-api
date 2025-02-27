namespace NotificationService.Application.Contracts.DTOs.Responses;

public class NotificationDetailDto : NotificationDto
{
    public object? Request { get; set; }
    public string? Content {get; set;}
    public new ICollection<AttachmentDto>? Attachments { get; set; }

}