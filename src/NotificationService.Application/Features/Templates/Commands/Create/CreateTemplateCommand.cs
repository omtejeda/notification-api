using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.RequestDtos;

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