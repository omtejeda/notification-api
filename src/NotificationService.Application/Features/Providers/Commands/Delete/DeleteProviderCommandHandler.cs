using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Providers.Commands.Delete;

public class DeleteProviderCommandHandler(IProviderService providerService) : ICommandHandler<DeleteProviderCommand>
{
    private readonly IProviderService _providerService = providerService;

    public async Task Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        await _providerService.DeleteProvider(request.ProviderId, request.Owner);
    }
}