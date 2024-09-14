using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Commands.UpdateContent;

public class UpdateTemplateContentCommand : ICommand
{
    public string? TemplateId { get; set; }
    public string? Base64Content { get; set; }
    public string? Owner { get; set; }
}