using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Providers.Commands.AddToWhiteList;

public record AddToWhiteListCommand : ICommand
{
    public AddToWhiteListCommand(string? providerId, string? recipient, string? owner)
    {
        ProviderId = providerId;
        Recipient = recipient;
        Owner = owner;
    }
    
    public string? ProviderId { get; init; }
    public string? Recipient { get; init; }
    public string? Owner { get; set;}
}