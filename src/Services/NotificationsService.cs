using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NotificationService.Entities;
using NotificationService.Interfaces;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Dtos.Responses;
using NotificationService.Repositories;
using NotificationService.Enums;
using NotificationService.Exceptions;
using NotificationService.Services.Interfaces;
using System.Linq;
using LinqKit;

namespace NotificationService.Services
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

        public async Task<string> RegisterNotification(NotificationType type, string toDestination, string templateName, string platformName, string providerName, bool success, string message, string owner, object request, string parentNotificationId = null, List<Microsoft.AspNetCore.Http.IFormFile> attachments = null)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid().ToString(),
                Type = type,
                ToDestination = toDestination,
                TemplateName = templateName,
                PlatformName = platformName,
                ProviderName = providerName,
                Date = Utils.SystemUtil.GetSystemDate(),
                Success = success,
                Message = message,
                HasAttachments = attachments?.Any() ?? false,
                Attachments = GetAttachmentsCollection(attachments)?.ToList(),
                Request = request,
                ParentNotificationId = parentNotificationId,
                CreatedBy = owner
            };

            await _notificationRepository.InsertOneAsync(notification);
            return notification.NotificationId;
        }

        public async Task<FinalResponseDTO<IEnumerable<NotificationDTO>>> GetNotifications(Expression<Func<Notification, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Notification>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var (notifications, pagination) = await _notificationRepository.FindAsync(filter, page, pageSize);
            var notificationsDTO = _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
            var paginationDTO = _mapper.Map<PaginationDTO>(pagination);

            return new FinalResponseDTO<IEnumerable<NotificationDTO>>( (int) ErrorCode.OK, notificationsDTO, paginationDTO);
        }

        public async Task<FinalResponseDTO<NotificationDTO>> GetNotificationById(string notificationId, string owner)
        {
            var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);

            if (notification == null) return default;

            if (notification.CreatedBy != owner)
                throw new RuleValidationException($"Notification was not created by platform {owner}");

            var notificationDTO = _mapper.Map<NotificationDTO>(notification);

            return new FinalResponseDTO<NotificationDTO>((int) ErrorCode.OK, notificationDTO);
        }

        private IEnumerable<Entities.Attachment> GetAttachmentsCollection(List<Microsoft.AspNetCore.Http.IFormFile> attachments)
        {
            if (attachments == null) yield break;

            foreach(var attachment in attachments)
            {
                yield return new Entities.Attachment
                {
                    FileName = attachment.FileName,
                    ContentType = attachment.ContentType,
                    Length = attachment.Length
                };
            }
        }
    }
}