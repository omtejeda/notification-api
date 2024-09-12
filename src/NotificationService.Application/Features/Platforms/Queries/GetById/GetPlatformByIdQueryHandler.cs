using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Platforms.Queries.GetById;

public class GetPlatformByIdQueryHandler(IPlatformService platformService)
    : IQueryHandler<GetPlatformByIdQuery, BaseResponse<PlatformDto>>
{
    private readonly IPlatformService _platformService = platformService;

    public async Task<BaseResponse<PlatformDto>> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        return await _platformService.GetPlatformById(request.PlatformId, request.Owner);
    }
}