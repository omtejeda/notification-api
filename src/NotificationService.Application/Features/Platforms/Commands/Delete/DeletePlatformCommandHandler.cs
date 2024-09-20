using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using MediatR;
using NotificationService.Application.Features.Platforms.Events.Deleted;

namespace NotificationService.Application.Features.Platforms.Commands.Delete;

public class DeletePlatformCommandHandler(IPlatformService platformService, IMediator mediator) : ICommandHandler<DeletePlatformCommand>
{
    private readonly IPlatformService _platformService = platformService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        await _platformService.DeletePlatform(request.PlatformId, request.Owner);

        await PublishEvent(request.PlatformId);
    }

    private async Task PublishEvent(string platformId)
    {
        var eventToPublish = new PlatformDeletedEvent(platformId);
        await _mediator.Publish(eventToPublish);
    }
}