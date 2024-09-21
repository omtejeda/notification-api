using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Notifications.Events.Resent;

/// <summary>
/// Handles the NotificationResentEvent and logs the details of the created platform.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class NotificationResentEventHandler(ILogger<NotificationResentEventHandler> logger)
    : IEventHandler<NotificationResentEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(NotificationResentEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            string result = notification.Success
            ? "success"
            : "failed";
            
            _logger.LogInformation("Notification with Id {platformId} has been resent with {result} result", notification.NotificationId, result);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(NotificationResentEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}