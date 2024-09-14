using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Contracts.Interfaces.Factories;
using NotificationService.Common.Exceptions;

namespace NotificationService.Application.Features.Notifications.Queries.Export;

public class ExportNotificationQueryHandler(IExportNotificationsFactory exportNotificationsFactory)
    : IQueryHandler<ExportNotificationQuery, ExportNotificationResponseDto>
{
    private readonly IExportNotificationsFactory _exportNotificationsFactory = exportNotificationsFactory;
    

    public async Task<ExportNotificationResponseDto> Handle(ExportNotificationQuery request, CancellationToken cancellationToken)
    {
        var exportService = _exportNotificationsFactory.Create(request.Format) 
            ?? throw new RuleValidationException($"The specified format '{request.Format}' doesn't exist");
        
        var response = await exportService.Export(request.NotificationId!, request.Owner!) 
            ?? throw new RuleValidationException("The specified notificationId doesn't exist");

        return response;
    }
}