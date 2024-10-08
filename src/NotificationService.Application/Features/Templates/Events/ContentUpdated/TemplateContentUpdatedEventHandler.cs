using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.ContentUpdated;

/// <summary>
/// Handles the TemplateContentUpdatedEvent and logs the details of the updated template content.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class TemplateContentUpdatedEventHandler(ILogger<TemplateContentUpdatedEventHandler> logger)
    : IEventHandler<TemplateContentUpdatedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(TemplateContentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("The content of the template with Id {TemplateId} was updated", notification.TemplateId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(TemplateContentUpdatedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}