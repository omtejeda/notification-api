using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.Factories;

namespace NotificationService.Application.Features.Notifications.Factories;

public class ExportNotificationsFactory : IExportNotificationsFactory
{

    private readonly IEnumerable<IExportNotificationsService> _exportNotificationsServices;
    public ExportNotificationsFactory(IEnumerable<IExportNotificationsService> exportNotificationsServices)
    {
        _exportNotificationsServices = exportNotificationsServices;
    }

    public IExportNotificationsService Create(ExportFormat exportFormat)
    {
        return _exportNotificationsServices.FirstOrDefault(x => x.ExportFormat == exportFormat)
            ?? throw new ArgumentException("Could not get a valid exporter");
    }
}