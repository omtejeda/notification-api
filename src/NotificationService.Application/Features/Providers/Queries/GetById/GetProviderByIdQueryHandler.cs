using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Providers.Queries.GetById;

public class GetProviderByIdQueryHandler(IProviderService providerService)
    : IQueryHandler<GetProviderByIdQuery, BaseResponse<ProviderDto>>
{
    private readonly IProviderService _providerService = providerService;

    public async Task<BaseResponse<ProviderDto>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _providerService.GetProviderById(request.ProviderId, request.Owner);
    }
}