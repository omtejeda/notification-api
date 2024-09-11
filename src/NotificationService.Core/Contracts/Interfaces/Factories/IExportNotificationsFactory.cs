using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Domain.Enums;

namespace NotificationService.Core.Contracts.Interfaces.Factories
{
    public interface IExportNotificationsFactory
    {
        IExportNotificationsService Create(ExportFormat exportFormat);
    }
}