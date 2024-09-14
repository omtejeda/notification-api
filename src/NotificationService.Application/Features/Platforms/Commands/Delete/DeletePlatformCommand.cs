using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Commands.Delete;

public record DeletePlatformCommand : ICommand
{
    public string? PlatformId { get; init; }
    public string? Owner { get; set;}
}