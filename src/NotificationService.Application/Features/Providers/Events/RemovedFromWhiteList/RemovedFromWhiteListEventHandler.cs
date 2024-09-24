using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.RemovedFromWhiteList;

/// <summary>
/// Handles the ProviderDeletedEvent and logs the details of the recipient removed from whitelist.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class RemovedFromWhiteListEventHandler(ILogger<RemovedFromWhiteListEventHandler> logger)
    : IEventHandler<RemovedFromWhiteListEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(RemovedFromWhiteListEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Recipient {recipient} has been removed from whitelist for Provider with Id {providerId}", notification.Recipient, notification.ProviderId);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(RemovedFromWhiteListEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}