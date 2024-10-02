using Microsoft.Extensions.Logging;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.Deleted;

/// <summary>
/// Handles the TemplateDeletedEvent and logs the details of the deleted template.
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
            _logger.LogInformation("Template with Id {TemplateId} has been deleted", notification.TemplateId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(TemplateDeletedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}