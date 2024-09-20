using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Events.Deleted;

internal sealed class PlatformDeletedEvent : IEvent
{
    internal PlatformDeletedEvent(string platformId)
    {
        PlatformId = platformId;
    }

    public string PlatformId { get; }
}