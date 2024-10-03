using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using MediatR;
using NotificationService.Application.Features.Providers.Events.AddedToWhiteList;

namespace NotificationService.Application.Features.Providers.Commands.AddToWhiteList;

public class AddToWhiteListCommandHandler(IProviderService providerService, IMediator mediator)
    : ICommandHandler<AddToWhiteListCommand>
{
    private readonly IProviderService _providerService = providerService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(AddToWhiteListCommand request, CancellationToken cancellationToken)
    {
        await _providerService.AddToWhiteList(request.ProviderId, request.Owner, request.Recipient);

        await _mediator.Publish(new AddedToWhiteListEvent(request.ProviderId, request.Recipient), CancellationToken.None);
    }
}