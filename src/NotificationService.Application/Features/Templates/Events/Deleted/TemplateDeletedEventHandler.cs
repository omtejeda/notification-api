using Microsoft.Extensions.Logging;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.Deleted;

/// <summary>
/// Handles the PlatformDeletedEvent and logs the details of the created platform.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class TemplateDeletedEventHandler(ILogger<TemplateDeletedEventHandler> logger)
    : IEventHandler<TemplateDeletedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(TemplateDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Template with Id {templateId} has been deleted", notification.TemplateId);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(TemplateDeletedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}