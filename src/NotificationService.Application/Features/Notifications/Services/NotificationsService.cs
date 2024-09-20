using AutoMapper;
using LinqKit;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.Interfaces.Services;
using System.Linq.Expressions;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Domain.Models;
using NotificationService.Application.Utils;

namespace NotificationService.Application.Features.Notifications.Services;

public class NotificationsService : INotificationsService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Notification> _notificationRepository;

    public NotificationsService(IRepository<Notification> notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<string> RegisterNotification(Notification notification)
    {
        if (notification is null) return default!;
        
        await _notificationRepository.InsertOneAsync(notification);
        return notification.NotificationId;
    }

    public async Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, FilterOptions filterOptions)
    {
        var filterByOwner = PredicateBuilder.New<Notification>().And(x => x.CreatedBy == owner).Expand();
        filter = filter.And(filterByOwner);
        
        var (notifications, pagination) = await _notificationRepository.FindAsync(filter, filterOptions);
        var notificationsDTO = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        var paginationDto = _mapper.Map<PaginationDto>(pagination);

        return BaseResponse<IEnumerable<NotificationDto>>.Success(notificationsDTO, paginationDto);
    }

    public async Task<BaseResponse<NotificationDetailDto>> GetNotificationById(string notificationId, string owner)
    {
        var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);
        if (notification is null) return default!;

        Guard.NotificationWasCreatedByRequester(notification?.CreatedBy, owner);
        var notificationDto = _mapper.Map<NotificationDetailDto>(notification);

        return BaseResponse<NotificationDetailDto>.Success(notificationDto);
    }

    public async IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDto> attachments)
    {
        if (attachments is null) yield break;

        foreach (var attachment in attachments)
        {
            var result = _mapper.Map<AttachmentContentDto>(attachment);
            var fileData = await _notificationRepository.GetFileByNameAsync(attachment.FileName);
            result.EncodedContent =  fileData is null ? "" : Convert.ToBase64String(fileData);
            yield return result;
        }
    }

    public async Task<(byte[], string)> GetNotificationAttachment(string notificationId, string fileName, string owner)
    {
        var notification = await GetNotificationById(notificationId, owner);
        Guard.NotificationIsNotNull(notification);

        var attachment = notification?.Data?.Attachments?.FirstOrDefault(x => x.FileName == fileName);
        Guard.AttachmentExists(attachment, fileName);

        var file = await _notificationRepository.GetFileByNameAsync(fileName);
        return (file, attachment!.ContentType);
    }

    public async Task SaveAttachments(IEnumerable<Attachment> attachments)
    {
        var tasks = new List<Task>();

        attachments.ForEach(x =>
        {
            ArgumentNullException.ThrowIfNull(x.FormFile, nameof(x.FormFile));
            tasks.Add(_notificationRepository.UploadFileAsync(x.FormFile.OpenReadStream(), x.FileName));
        });

        await Task.WhenAll(tasks);
    }
}