using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Webhooks.Events.EmailContentSaved;

/// <summary>
/// Handles the EmailContentSavedEvent and logs the details of the saved email content.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class EmailContentSavedEventHandler(ILogger<EmailContentSavedEventHandler> logger)
    : IEventHandler<EmailContentSavedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(EmailContentSavedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            string result = notification.Success
            ? "success"
            : "failed";
            
            _logger.LogInformation("Email content for notification with Id {notification} has been saved with {result} result", notification.NotificationId, result);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(EmailContentSavedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}