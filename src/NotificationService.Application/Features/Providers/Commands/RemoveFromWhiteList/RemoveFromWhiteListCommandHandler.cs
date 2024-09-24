using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using MediatR;
using NotificationService.Application.Features.Providers.Events.RemovedFromWhiteList;

namespace NotificationService.Application.Features.Providers.Commands.RemoveFromWhiteList;

public class RemoveFromWhiteListCommandHandler(IProviderService providerService, IMediator mediator) 
    : ICommandHandler<RemoveFromWhiteListCommand>
{
    private readonly IProviderService _providerService = providerService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(RemoveFromWhiteListCommand request, CancellationToken cancellationToken)
    {
        await _providerService.DeleteFromWhiteList(request.ProviderId, request.Owner, request.Recipient);

        await _mediator.Publish(new RemovedFromWhiteListEvent(request.ProviderId, request.Recipient));
    }
}