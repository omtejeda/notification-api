using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Commands.Delete;

public record DeleteTemplateCommand : ICommand
{
    public string? TemplateId { get; init; }
    public string? Owner { get; set;}
}