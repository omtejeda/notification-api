namespace NotificationService.Application.Contracts.DTOs.Responses;

public class AttachmentContentDto : AttachmentDto
{
    public string? EncodedContent { get; set; }
}