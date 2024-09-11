using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Core.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Contracts.Interfaces.Services
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