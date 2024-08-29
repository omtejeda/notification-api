using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Common.Entities;
using NotificationService.Common.Models;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface INotificationsService
    {
        Task<string> RegisterNotification(Notification notification);
        Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize, string sort);
        Task<BaseResponse<NotificationDetailDto>> GetNotificationById(string notificationId, string owner);
        Task<(byte[], string)> GetNotificationAttachment(string notificationId, string fileName, string owner);
        IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDto> attachments) ;
        Task SaveAttachments(IEnumerable<Attachment> attachments);
    }
}