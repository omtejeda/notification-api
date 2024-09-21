using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.Deleted;

internal sealed class TemplateDeletedEvent : IEvent
{
    internal TemplateDeletedEvent(string templateId)
    {
        TemplateId = templateId;
    }

    public string TemplateId { get; }
}