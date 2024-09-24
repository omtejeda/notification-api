using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Notifications.Events.Resent;

internal sealed class NotificationResentEvent : IEvent
{
    internal NotificationResentEvent(string notificationId, bool? success)
    {
        NotificationId = notificationId;
        Success = success ?? false;
    }

    public string NotificationId { get; }
    public bool Success { get; }
}