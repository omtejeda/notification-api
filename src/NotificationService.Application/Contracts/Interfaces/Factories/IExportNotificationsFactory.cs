using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Contracts.Interfaces.Factories;

/// <summary>
/// Factory interface for creating instances of <see cref="IExportNotificationsService"/>.
/// This factory allows for the creation of export services based on the specified export format.
/// </summary>
public interface IExportNotificationsFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IExportNotificationsService"/> based on the provided export format.
    /// </summary>
    /// <param name="exportFormat">The format in which notifications should be exported.</param>
    /// <returns>An instance of <see cref="IExportNotificationsService"/> configured for the specified format.</returns>
    IExportNotificationsService Create(ExportFormat exportFormat);
}