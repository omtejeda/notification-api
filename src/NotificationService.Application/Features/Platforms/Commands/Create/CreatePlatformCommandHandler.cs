using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Services;
using MediatR;
using NotificationService.Application.Features.Platforms.Events.Created;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Platforms.Commands.Create;

public class CreatePlatformCommandHandler(IPlatformService platformService, IMediator mediator) 
    : ICommandHandler<CreatePlatformCommand, BaseResponse<PlatformDto>>
{
    private readonly IPlatformService _platformService = platformService;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<PlatformDto>> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var result = await _platformService.CreatePlatform(request.Name, request.Description, request.Owner);

        await _mediator.Publish(new PlatformCreatedEvent(result.Data), CancellationToken.None);
        return result;
    }
}