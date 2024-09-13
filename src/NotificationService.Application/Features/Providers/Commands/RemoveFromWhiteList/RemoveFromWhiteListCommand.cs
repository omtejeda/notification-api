using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Features.Providers.Commands.RemoveFromWhiteList;

public record RemoveFromWhiteListCommand : ICommand
{
    public RemoveFromWhiteListCommand(string? providerId, string? recipient, string? owner)
    {
        ProviderId = providerId;
        Recipient = recipient;
        Owner = owner;
    }
    
    public string? ProviderId { get; init; }
    public string? Recipient { get; init; }
    public string? Owner { get; set;}
}