using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Events.NotificationSent;

/// <summary>
/// Handles the NotificationSentEvent and logs the details of the sent notification.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class NotificationSentEventHandler(ILogger<NotificationSentEventHandler> logger) : IEventHandler<NotificationSentEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(NotificationSentEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            string result = notification.Success
            ? "success"
            : "failed";
            
            _logger.LogInformation("Notification with Id {NotificationId} of type {NotificationType} has been sent to {ToDestination} with {Result} result",
            notification.NotificationId,
            notification.NotificationType.ToString(),
            notification.ToDestination,
            result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(NotificationSentEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}