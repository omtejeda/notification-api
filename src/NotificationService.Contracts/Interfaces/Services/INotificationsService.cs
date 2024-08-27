using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Common.Entities;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface INotificationsService
    {
        Task<string> RegisterNotification(Notification notification);
        Task<FinalResponseDTO<IEnumerable<NotificationDTO>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize, string sort);
        Task<FinalResponseDTO<NotificationDetailDto>> GetNotificationById(string notificationId, string owner);
        Task<(byte[], string)> GetNotificationAttachment(string notificationId, string fileName, string owner);
        IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDTO> attachments) ;
        Task SaveAttachments(IEnumerable<Attachment> attachments);
    }
}