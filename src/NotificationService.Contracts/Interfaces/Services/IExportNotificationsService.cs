using System.Threading.Tasks;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface IExportNotificationsService
    {
        ExportFormat ExportFormat { get; }

        Task<ExportNotificationResponseDto> Export(string notificationId, string owner);
    } 
}