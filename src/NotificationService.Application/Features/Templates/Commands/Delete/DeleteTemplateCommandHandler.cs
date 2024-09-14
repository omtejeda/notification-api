using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Templates.Commands.Delete;

public class DeleteTemplateCommandHandler(ITemplateService templateService) : ICommandHandler<DeleteTemplateCommand>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        await _templateService.DeleteTemplate(request.TemplateId!, request.Owner!);
    }
}