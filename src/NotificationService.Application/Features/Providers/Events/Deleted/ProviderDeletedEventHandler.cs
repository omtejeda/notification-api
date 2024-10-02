using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.Deleted;

/// <summary>
/// Handles the ProviderDeletedEvent and logs the details of the deleted provider.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class ProviderDeletedEventHandler(ILogger<ProviderDeletedEventHandler> logger)
    : IEventHandler<ProviderDeletedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(ProviderDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Provider with Id {ProviderId} has been deleted", notification.ProviderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(ProviderDeletedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}