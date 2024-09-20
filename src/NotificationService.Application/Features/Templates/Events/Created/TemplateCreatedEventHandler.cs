using Microsoft.Extensions.Logging;
using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Templates.Events.Created;

/// <summary>
/// Handles the TemplateCreatedEvent and logs the details of the created platform.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class TemplateCreatedEventHandler(ILogger<TemplateCreatedEventHandler> logger)
    : IEventHandler<TemplateCreatedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(TemplateCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            TemplateDto? data = notification.Data;
            _logger.LogInformation("Template created with Id {templateId} and Name {templateName}", data?.TemplateId, data?.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(TemplateCreatedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}