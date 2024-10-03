using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using MediatR;
using NotificationService.Application.Features.Providers.Events.Deleted;

namespace NotificationService.Application.Features.Providers.Commands.Delete;

public class DeleteProviderCommandHandler(IProviderService providerService, IMediator mediator) 
    : ICommandHandler<DeleteProviderCommand>
{
    private readonly IProviderService _providerService = providerService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        await _providerService.DeleteProvider(request.ProviderId, request.Owner);
        
        await _mediator.Publish(new ProviderDeletedEvent(request.ProviderId), CancellationToken.None);
    }
}