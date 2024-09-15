using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Features.Webhooks.Commands.SaveEmailContent;

namespace NotificationService.Application.Features.Webhooks.Commands.Create;

public class SaveEmailContentCommandHandler(IWebhooksService webhookService) 
    : ICommandHandler<SaveEmailContentCommand, bool>
{
    private readonly IWebhooksService _webhookService = webhookService;

    public async Task<bool> Handle(SaveEmailContentCommand request, CancellationToken cancellationToken)
    {
        return await _webhookService.SaveEmailContent(request.Html, request.Subject, request.Headers);
    }
}