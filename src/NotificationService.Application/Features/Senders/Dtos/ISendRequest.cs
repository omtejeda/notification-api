namespace NotificationService.Application.Features.Senders.Dtos;

public interface ISendRequest
{
    public string? ParentNotificationId { get; set; }
}