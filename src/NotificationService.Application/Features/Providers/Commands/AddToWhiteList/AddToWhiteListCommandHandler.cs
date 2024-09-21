using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Providers.Commands.AddToWhiteList;

public class AddToWhiteListCommandHandler(IProviderService providerService) : ICommandHandler<AddToWhiteListCommand>
{
    private readonly IProviderService _providerService = providerService;

    public async Task Handle(AddToWhiteListCommand request, CancellationToken cancellationToken)
    {
        await _providerService.AddToWhiteList(request.ProviderId, request.Owner, request.Recipient);
    }
}