namespace NotificationService.Application.Dtos;

public interface ISendRequest
{
    public string? ParentNotificationId { get; set; }
}