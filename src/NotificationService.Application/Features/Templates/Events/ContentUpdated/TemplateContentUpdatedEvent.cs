using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.ContentUpdated;

internal sealed class TemplateContentUpdatedEvent : IEvent
{
    internal TemplateContentUpdatedEvent(string templateId)
    {
        TemplateId = templateId;
    }

    public string TemplateId { get; }
}