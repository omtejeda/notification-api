using NotificationService.Domain.Enums;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Events.NotificationSent;

internal sealed class NotificationSentEvent : IEvent
{
    public string? NotificationId { get; init; }
    public NotificationType NotificationType { get; init; }
    public string? ToDestination { get; init; }
    public bool Success { get; init; }
}