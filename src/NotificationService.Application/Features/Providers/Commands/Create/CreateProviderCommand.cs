using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Providers.Commands.Create;

public record CreateProviderCommand : ICommand<BaseResponse<ProviderDto>>
{
    public CreateProviderCommand(CreateProviderRequestDto requestDto, string owner)
    {
        RequestDto = requestDto;
        Owner = owner;
    }
    
    public CreateProviderRequestDto RequestDto { get; set; }
    public string Owner { get; set; }
}