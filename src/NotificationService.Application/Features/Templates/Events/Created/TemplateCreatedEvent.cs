using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.Created;

internal sealed class TemplateCreatedEvent : IEvent
{
    internal TemplateCreatedEvent(TemplateDto? data)
    {
        Data = data;
    }

    public TemplateDto? Data { get; }
}