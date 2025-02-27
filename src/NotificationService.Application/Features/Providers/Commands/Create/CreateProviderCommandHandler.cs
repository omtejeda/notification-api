using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;
using MediatR;
using NotificationService.Application.Features.Providers.Events.Created;

namespace NotificationService.Application.Features.Providers.Commands.Create;

public class CreateProviderCommandHandler(IProviderService providerService, IMediator mediator) 
    : ICommandHandler<CreateProviderCommand, BaseResponse<ProviderDto>>
{
    private readonly IProviderService _providerService = providerService;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<ProviderDto>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var result = await _providerService.CreateProvider(request.RequestDto, request.Owner);

        await _mediator.Publish(new ProviderCreatedEvent(result.Data), CancellationToken.None);
        return result;
    }
}