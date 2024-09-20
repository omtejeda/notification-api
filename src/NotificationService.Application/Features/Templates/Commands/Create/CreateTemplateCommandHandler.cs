using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Templates.Events.Created;
using MediatR;

namespace NotificationService.Application.Features.Templates.Commands.Create;

public class CreateTemplateCommandHandler(ITemplateService templateService, IMediator mediator) 
    : ICommandHandler<CreateTemplateCommand, BaseResponse<TemplateDto>>
{
    private readonly ITemplateService _templateService = templateService;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<TemplateDto>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _templateService.CreateTemplate(request.RequestDto, request.Owner);

        await PublishEvent(result.Data);
        return result;
    }

    private async Task PublishEvent(TemplateDto? data)
    {
        var eventToPublish = new TemplateCreatedEvent(data);
        await _mediator.Publish(eventToPublish);
    }
}