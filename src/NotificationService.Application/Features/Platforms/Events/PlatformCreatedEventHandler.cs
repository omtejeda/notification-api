using Microsoft.Extensions.Logging;
using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Events;

/// <summary>
/// Handles the PlatformCreatedEvent and logs the details of the created platform.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class PlatformCreatedEventHandler(ILogger<PlatformCreatedEventHandler> logger)
    : IEventHandler<PlatformCreatedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(PlatformCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            PlatformDto? data = notification.Data;
            _logger.LogInformation("Platform created with Id {platformId} and Name {platformName}", data?.PlatformId, data?.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(PlatformCreatedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}