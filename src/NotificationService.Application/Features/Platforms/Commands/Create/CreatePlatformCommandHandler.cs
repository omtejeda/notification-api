using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Platforms.Commands.Create;

public class CreatePlatformCommandHandler(IPlatformService platformService) 
    : ICommandHandler<CreatePlatformCommand, BaseResponse<PlatformDto>>
{
    private readonly IPlatformService _platformService = platformService;

    public async Task<BaseResponse<PlatformDto>> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var result = await _platformService.CreatePlatform(request.Name!, request.Description!, request?.Owner!);
        return result;
    }
}