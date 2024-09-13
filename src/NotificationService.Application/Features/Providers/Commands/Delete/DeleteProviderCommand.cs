using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Features.Providers.Commands.Delete;

public record DeleteProviderCommand : ICommand
{
    public string? ProviderId { get; init; }
    public string? Owner { get; set;}
}