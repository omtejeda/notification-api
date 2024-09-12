using System.Threading.Tasks;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Contracts.Interfaces.Services
{
    public interface IExportNotificationsService
    {
        ExportFormat ExportFormat { get; }

        Task<ExportNotificationResponseDto> Export(string notificationId, string owner);
    } 
}