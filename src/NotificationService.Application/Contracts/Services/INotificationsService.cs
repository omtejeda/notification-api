using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Services;

/// <summary>
/// Defines the contract for managing notifications, including registering, retrieving, and handling attachments.
/// </summary>
public interface INotificationsService
{
    /// <summary>
    /// Registers a notification sent in the database.
    /// </summary>
    /// <param name="notification">The notification entity to register.</param>
    /// <returns>A string representing the unique identifier of the registered notification.</returns>
    Task<string> RegisterNotification(Notification notification);

    /// <summary>
    /// Retrieves a list of notifications that match the specified filter criteria.
    /// </summary>
    /// <param name="filter">The filter expression to apply to the notifications.</param>
    /// <param name="owner">The owner of the notifications.</param>
    /// <param name="filterOptions">Additional filtering and pagination options.</param>
    /// <returns>A <see cref="BaseResponse{IEnumerable{NotificationDto}}"/> containing the matching notifications.</returns>
    Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, FilterOptions filterOptions);

    /// <summary>
    /// Retrieves a specific notification by its identifier.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification.</param>
    /// <param name="owner">The owner of the notification.</param>
    /// <returns>A <see cref="BaseResponse{NotificationDetailDto}"/> containing the details of the notification.</returns>
    Task<BaseResponse<NotificationDetailDto>> GetNotificationById(string notificationId, string owner);

    /// <summary>
    /// Retrieves a specific attachment for a notification based on the file name.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification.</param>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <param name="owner">The owner of the notification.</param>
    /// <returns>A tuple containing the file content as a byte array and the file's MIME type.</returns>
    Task<(byte[], string)> GetNotificationAttachment(string notificationId, string fileName, string owner);

    /// <summary>
    /// Retrieves the base64-encoded content of multiple attachments asynchronously.
    /// </summary>
    /// <param name="attachments">A collection of attachment DTOs representing the attachments to retrieve.</param>
    /// <returns>An asynchronous enumerable of <see cref="AttachmentContentDto"/> objects containing the base64 content of the attachments.</returns>
    IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDto> attachments);

    /// <summary>
    /// Saves the specified attachments.
    /// </summary>
    /// <param name="attachments">A collection of attachment entities to save.</param>
    Task SaveAttachments(IEnumerable<Attachment> attachments);
}