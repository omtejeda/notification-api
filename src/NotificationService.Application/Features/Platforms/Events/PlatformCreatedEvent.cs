using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Events;

internal sealed class PlatformCreatedEvent : IEvent
{
    internal PlatformCreatedEvent(PlatformDto? data)
    {
        Data = data;
    }

    public PlatformDto? Data { get; }
}