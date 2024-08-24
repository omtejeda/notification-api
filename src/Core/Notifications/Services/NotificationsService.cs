using AutoMapper;
using LinqKit;
using NotificationService.Core.Common.Dtos;
using NotificationService.Core.Common.Entities;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Core.Notifications.Services
{
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
            if (notification is null) return default;
            
            await _notificationRepository.InsertOneAsync(notification);
            return notification.NotificationId;
        }

        private static IReadOnlyList<string> GetSortItems(string sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return Array.Empty<string>();

            const char delimiter = ',';

            return sort.Contains(delimiter) ?
                sort.Split(delimiter).ToList() : new List<string> { sort };
        }

        public async Task<FinalResponseDTO<IEnumerable<NotificationDTO>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize, string sort)
        {
            var filterByOwner = PredicateBuilder.New<Notification>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var sortBy = GetSortItems(sort);

            var (notifications, pagination) = await _notificationRepository.FindAsync(filter, page, pageSize, sortBy);
            var notificationsDTO = _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
            var paginationDTO = _mapper.Map<PaginationDTO>(pagination);

            return new FinalResponseDTO<IEnumerable<NotificationDTO>>( (int) ErrorCode.OK, notificationsDTO, paginationDTO);
        }

        public async Task<FinalResponseDTO<NotificationDetailDto>> GetNotificationById(string notificationId, string owner)
        {
            var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);

            if (notification is null) return default;

            if (notification.CreatedBy != owner)
                throw new RuleValidationException($"Notification was not created by platform {owner}");

            var notificationDTO = _mapper.Map<NotificationDetailDto>(notification);

            return new FinalResponseDTO<NotificationDetailDto>((int) ErrorCode.OK, notificationDTO);
        }

        public async IAsyncEnumerable<AttachmentContentDto> GetAttachmentsAsBase64(IEnumerable<AttachmentDTO> attachments)
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
            if (notification is null)
                throw new RuleValidationException("Notification does not exist");

            var attachment = notification?.Data?.Attachments?.FirstOrDefault(x => x.FileName == fileName);
            if (attachment is null)
                throw new RuleValidationException($"Attachment: {fileName} not found");

            var file = await _notificationRepository.GetFileByNameAsync(fileName);
            return (file, attachment.ContentType);
        }

        public async Task SaveAttachments(IEnumerable<Attachment> attachments)
        {
            var tasks = new List<Task>();

            attachments.ForEach(x =>
            {
                tasks.Add(_notificationRepository.UploadFileAsync(x.FormFile.OpenReadStream(), x.FileName));
            });

            await Task.WhenAll(tasks);
        }
    }
}