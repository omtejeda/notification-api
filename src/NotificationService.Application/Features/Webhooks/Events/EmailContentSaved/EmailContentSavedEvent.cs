using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Webhooks.Events.EmailContentSaved;

internal sealed class EmailContentSavedEvent : IEvent
{
    internal EmailContentSavedEvent(string notificationId, bool? success)
    {
        NotificationId = notificationId;
        Success = success ?? false;
    }

    public string NotificationId { get; }
    public bool Success { get; }
}