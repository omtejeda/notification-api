using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.AddedToWhiteList;

/// <summary>
/// Handles the ProviderDeletedEvent and logs the details of the recipient added to whitelist.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class AddedToWhiteListEventHandler(ILogger<AddedToWhiteListEventHandler> logger)
    : IEventHandler<AddedToWhiteListEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(AddedToWhiteListEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Recipient {Recipient} has been added to whitelist for Provider with Id {ProviderId}", notification.Recipient, notification.ProviderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(AddedToWhiteListEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}