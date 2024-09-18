using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Contracts.Interfaces.Services;

public interface INotificationsService
{
    Task<string> RegisterNotification(Notification notification);
    Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize, string? sort);
    Task<BaseResponse<NotificationDetailDto>> GetNotificationById(string notificationId, string owner);
    Task<(byte[], string)> GetNotificationAttachment(string notificationId, string fileName, string owner);
    IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDto> attachments) ;
    Task SaveAttachments(IEnumerable<Attachment> attachments);
}