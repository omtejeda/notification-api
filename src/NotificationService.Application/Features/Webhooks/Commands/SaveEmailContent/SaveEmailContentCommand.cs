using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Webhooks.Commands.SaveEmailContent;

public record SaveEmailContentCommand : ICommand<bool>
{
    public string Email { get; init; } = string.Empty;
    public string To { get; init; } = string.Empty;
    public string From { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string Html { get; init; } = string.Empty;
    public string Headers {get; init; } = string.Empty;
}