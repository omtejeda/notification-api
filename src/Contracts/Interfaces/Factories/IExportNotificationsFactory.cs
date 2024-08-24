using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Common.Enums;

namespace NotificationService.Contracts.Interfaces.Factories
{
    public interface IExportNotificationsFactory
    {
        IExportNotificationsService Create(ExportFormat exportFormat);
    }
}