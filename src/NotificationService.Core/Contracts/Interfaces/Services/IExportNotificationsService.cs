using System.Threading.Tasks;
using NotificationService.Core.Contracts.ResponseDtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Core.Contracts.Interfaces.Services
{
    public interface IExportNotificationsService
    {
        ExportFormat ExportFormat { get; }

        Task<ExportNotificationResponseDto> Export(string notificationId, string owner);
    } 
}