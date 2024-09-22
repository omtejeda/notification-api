using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.RemovedFromWhiteList;

internal sealed class RemovedFromWhiteListEvent : IEvent
{
    internal RemovedFromWhiteListEvent(string providerId, string recipient)
    {
        ProviderId = providerId;
        Recipient = recipient;
    }

    public string ProviderId { get; }
    public string Recipient { get; }
}