using Microsoft.Extensions.Logging;
using NotificationService.Application.Common.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Platforms.Events.Created;

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
            _logger.LogInformation("Platform created with Id {PlatformId} and Name {PlatformName}", data?.PlatformId, data?.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error ocurred while handling {EventName}, {ErrorMessage}", nameof(PlatformCreatedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}