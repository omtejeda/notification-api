using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.DTOs.Requests;
using MediatR;
using NotificationService.Application.Features.Templates.Events.ContentUpdated;

namespace NotificationService.Application.Features.Templates.Commands.UpdateContent;

public class UpdateTemplateContentCommandHandler(ITemplateService templateService, IMediator mediator) 
    : ICommandHandler<UpdateTemplateContentCommand>
{
    private readonly ITemplateService _templateService = templateService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(UpdateTemplateContentCommand request, CancellationToken cancellationToken)
    {
        var requestDto = new UpdateTemplateContentRequestDto(request.Base64Content);
        await _templateService.UpdateTemplateContent(request.TemplateId, requestDto, request.Owner);

        await _mediator.Publish(new TemplateContentUpdatedEvent(request.TemplateId), CancellationToken.None);
    }
}