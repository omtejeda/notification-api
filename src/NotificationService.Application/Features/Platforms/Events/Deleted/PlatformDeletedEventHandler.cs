using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Platforms.Events.Deleted;

/// <summary>
/// Handles the PlatformDeletedEvent and logs the details of the created platform.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class PlatformDeletedEventHandler(ILogger<PlatformDeletedEventHandler> logger)
    : IEventHandler<PlatformDeletedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(PlatformDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Platform with Id {PlatformId} has been deleted", notification.PlatformId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(PlatformDeletedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}