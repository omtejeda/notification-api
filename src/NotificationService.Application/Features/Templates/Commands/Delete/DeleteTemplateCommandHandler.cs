using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Templates.Events.Deleted;
using MediatR;

namespace NotificationService.Application.Features.Templates.Commands.Delete;

public class DeleteTemplateCommandHandler(ITemplateService templateService, IMediator mediator) : ICommandHandler<DeleteTemplateCommand>
{
    private readonly ITemplateService _templateService = templateService;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        await _templateService.DeleteTemplate(request.TemplateId, request.Owner);

        await _mediator.Publish(new TemplateDeletedEvent(request.TemplateId));
    }
}