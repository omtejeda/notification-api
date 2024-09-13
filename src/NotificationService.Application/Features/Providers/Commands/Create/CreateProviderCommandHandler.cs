using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

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