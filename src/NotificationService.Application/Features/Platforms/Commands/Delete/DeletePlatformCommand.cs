using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Commands.Delete;

public record DeletePlatformCommand : ICommand
{
    public string PlatformId { get; init; } = string.Empty;
    public string Owner { get; init; } = string.Empty;
}