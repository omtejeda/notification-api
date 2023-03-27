using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Entities;
using NotificationService.Dtos;
using NotificationService.Enums;

namespace NotificationService.Services.Interfaces
{
    public interface INotificationsService
    {
        Task<string> RegisterNotification(NotificationType type, string toDestination, string templateName, string platformName, string providerName, bool success, string message, string owner, object request, string parentNotificationId = null, List<Microsoft.AspNetCore.Http.IFormFile> attachments = null);
        Task<FinalResponseDTO<IEnumerable<NotificationDTO>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<NotificationDTO>> GetNotificationById(string notificationId, string owner);
    }
}