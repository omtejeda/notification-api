using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Commands.Delete;

public record DeleteTemplateCommand : ICommand
{
    public DeleteTemplateCommand(string templateId, string owner)
    {
        TemplateId = templateId;
        Owner = owner;
    }
    
    public string TemplateId { get; init; }
    public string Owner { get; init; }
}