using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Commands.AddToWhiteList;

public record AddToWhiteListCommand : ICommand
{
    public AddToWhiteListCommand(string providerId, string recipient, string owner)
    {
        ProviderId = providerId;
        Recipient = recipient;
        Owner = owner;
    }
    
    public string ProviderId { get; init; } = string.Empty;
    public string Recipient { get; init; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
}