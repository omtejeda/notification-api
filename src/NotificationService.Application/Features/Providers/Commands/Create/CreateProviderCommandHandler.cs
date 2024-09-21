using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Providers.Commands.Create;

public class CreateProviderCommandHandler(IProviderService providerService) 
    : ICommandHandler<CreateProviderCommand, BaseResponse<ProviderDto>>
{
    private readonly IProviderService _providerService = providerService;

    public async Task<BaseResponse<ProviderDto>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var result = await _providerService.CreateProvider(request.RequestDto, request.Owner);
        return result;
    }
}