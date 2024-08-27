using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Common.Enums;

namespace NotificationService.Contracts.Interfaces.Factories
{
    public interface IExportNotificationsFactory
    {
        IExportNotificationsService Create(ExportFormat exportFormat);
    }
}