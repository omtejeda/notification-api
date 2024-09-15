namespace NotificationService.Application.Senders.Dtos;

public interface ISendRequest
{
    public string? ParentNotificationId { get; set; }
}