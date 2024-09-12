using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Platforms.Commands.Delete;

public class DeleteCommandHandler(IPlatformService platformService) : ICommandHandler<DeletePlatformCommand>
{
    private readonly IPlatformService _platformService = platformService;

    public Task Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
    {
        return _platformService.DeletePlatform(request.PlatformId, request.Owner);
    }
}