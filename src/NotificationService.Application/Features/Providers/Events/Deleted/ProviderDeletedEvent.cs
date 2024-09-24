using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.Deleted;

internal sealed class ProviderDeletedEvent : IEvent
{
    internal ProviderDeletedEvent(string providerId)
    {
        ProviderId = providerId;
    }

    public string ProviderId { get; }
}