using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Templates.Events.Created;
using MediatR;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Templates.Commands.Create;

public class CreateTemplateCommandHandler(ITemplateService templateService, IMediator mediator) 
    : ICommandHandler<CreateTemplateCommand, BaseResponse<TemplateDto>>
{
    private readonly ITemplateService _templateService = templateService;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<TemplateDto>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _templateService.CreateTemplate(request.RequestDto, request.Owner);

        await _mediator.Publish(new TemplateCreatedEvent(result.Data));
        return result;
    }
}