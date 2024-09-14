using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Templates.Commands.Create;

public class CreateTemplateCommandHandler(ITemplateService templateService) 
    : ICommandHandler<CreateTemplateCommand, BaseResponse<TemplateDto>>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task<BaseResponse<TemplateDto>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _templateService.CreateTemplate(request.RequestDto!, request?.Owner!);
        return result;
    }
}