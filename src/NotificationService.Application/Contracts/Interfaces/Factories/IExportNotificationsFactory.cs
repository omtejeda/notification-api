using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Contracts.Interfaces.Factories;

public interface IExportNotificationsFactory
{
    IExportNotificationsService Create(ExportFormat exportFormat);
}