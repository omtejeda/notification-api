using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

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