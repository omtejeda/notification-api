using Microsoft.Extensions.Logging;
using NotificationService.Application.Common.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Providers.Events.Created;

/// <summary>
/// Handles the ProviderCreatedEvent and logs the details of the created provider.
/// This handler is designed to log information about the event while managing any potential issues 
/// internally, ensuring that any errors during logging or data handling do not affect the overall process.
/// </summary>
internal class ProviderCreatedEventHandler(ILogger<ProviderCreatedEventHandler> logger)
    : IEventHandler<ProviderCreatedEvent>
{
    private readonly ILogger _logger = logger;
    public async Task Handle(ProviderCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            ProviderDto? data = notification.Data;
            _logger.LogInformation("Provider created with Id {providerId} and Name {platformName}", data?.ProviderId, data?.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error ocurred while handling {eventName}, {errorMessage}", nameof(ProviderCreatedEvent), ex.Message);
        }

        await Task.CompletedTask;
    }
}