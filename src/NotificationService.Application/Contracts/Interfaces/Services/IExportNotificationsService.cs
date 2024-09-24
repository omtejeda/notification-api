using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Contracts.Interfaces.Services;

/// <summary>
/// Defines the contract for exporting notifications.
/// </summary>
public interface IExportNotificationsService
{
    /// <summary>
    /// The format in which the notification will be exported.
    /// </summary>
    ExportFormat ExportFormat { get; }

    /// <summary>
    /// Exports a notification based on its identifier and the owner's information.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification to be exported.</param>
    /// <param name="owner">The owner of the notification.</param>
    /// <returns>An <see cref="ExportNotificationResponseDto"/> containing the export details.</returns>
    Task<ExportNotificationResponseDto> Export(string notificationId, string owner);
}