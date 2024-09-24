using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Commands.Delete;

public record DeleteProviderCommand : ICommand
{
    public string ProviderId { get; init; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
}