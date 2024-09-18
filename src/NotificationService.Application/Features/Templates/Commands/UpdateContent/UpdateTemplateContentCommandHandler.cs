using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.RequestDtos;

namespace NotificationService.Application.Features.Templates.Commands.UpdateContent;

public class UpdateTemplateContentCommandHandler(ITemplateService templateService) 
    : ICommandHandler<UpdateTemplateContentCommand>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task Handle(UpdateTemplateContentCommand request, CancellationToken cancellationToken)
    {
        var requestDto = new UpdateTemplateContentRequestDto(request.Base64Content);
        await _templateService.UpdateTemplateContent(request.TemplateId, requestDto, request.Owner);
    }
}