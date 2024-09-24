using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.Senders.Events.NotificationSent;
using NotificationService.SharedKernel.Interfaces;

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
            
            _logger.LogInformation("Notification with Id {notificationId} of type {notificationType} has been sent to {toDestination} with {result} result",
            notification.NotificationId,
            notification.NotificationType.ToString(),
            notification.ToDestination,
            result);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(NotificationSentEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}