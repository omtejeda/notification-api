using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Webhooks.Commands.SaveEmailContent;
using MediatR;
using NotificationService.Application.Features.Webhooks.Events.EmailContentSaved;

namespace NotificationService.Application.Features.Webhooks.Commands.Create;

public class SaveEmailContentCommandHandler(IWebhooksService webhookService, IMediator mediator) 
    : ICommandHandler<SaveEmailContentCommand, bool>
{
    private readonly IWebhooksService _webhookService = webhookService;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> Handle(SaveEmailContentCommand request, CancellationToken cancellationToken)
    {
        var (success, notificationId) = await _webhookService.SaveEmailContent(request.Html, request.Subject, request.Headers);

        await _mediator.Publish(new EmailContentSavedEvent(notificationId, success));
        return success;
    }
}