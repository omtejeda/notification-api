using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.AddedToWhiteList;

internal sealed class AddedToWhiteListEvent : IEvent
{
    internal AddedToWhiteListEvent(string providerId, string recipient)
    {
        ProviderId = providerId;
        Recipient = recipient;
    }

    public string ProviderId { get; }
    public string Recipient { get; }
}