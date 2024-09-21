using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Common.Dtos;

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