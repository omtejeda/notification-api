using NotificationService.Application.Common.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.Created;

internal sealed class ProviderCreatedEvent : IEvent
{
    internal ProviderCreatedEvent(ProviderDto? data)
    {
        Data = data;
    }

    public ProviderDto? Data { get; }
}