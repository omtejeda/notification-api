using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Common.Dtos;

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