using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Templates.Commands.Create;

public class CreateTemplateCommand : ICommand<BaseResponse<TemplateDto>>
{
    public CreateTemplateCommand(CreateTemplateRequestDto requestDto, string owner)
    {
        RequestDto = requestDto;
        Owner = owner;
    }
    
    public CreateTemplateRequestDto RequestDto { get; set; }
    public string Owner { get; set; }
}