using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Providers.Commands.RemoveFromWhiteList;

public class RemoveFromWhiteListCommandHandler(IProviderService providerService) : ICommandHandler<RemoveFromWhiteListCommand>
{
    private readonly IProviderService _providerService = providerService;

    public async Task Handle(RemoveFromWhiteListCommand request, CancellationToken cancellationToken)
    {
        await _providerService.DeleteFromWhiteList(request.ProviderId, request.Owner, request.Recipient);
    }
}