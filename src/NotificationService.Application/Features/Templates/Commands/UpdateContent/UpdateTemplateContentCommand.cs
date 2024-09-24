using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Commands.UpdateContent;

public class UpdateTemplateContentCommand(string templateId, string base64Content, string owner) : ICommand
{
    public string TemplateId { get; set; } = templateId;
    public string Base64Content { get; set; } = base64Content;
    public string Owner { get; set; } = owner;
}