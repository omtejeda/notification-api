using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

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