using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LinqKit;
using NotificationService.Domain.Entities;
using NotificationService.Api.Utils;
using NotificationService.Domain.Enums;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Contracts.ResponseDtos;
using NotificationService.Core.Contracts.Interfaces.Repositories;
using NotificationService.Core.Contracts.Interfaces.Factories;
using NotificationService.Core.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Core.Dtos;
using NotificationService.Common.Utils;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class NotificationsController : ApiController
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly INotificationsService _notificationsService;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMessageSender _messageSender;
        private readonly IExportNotificationsFactory _exportNotificationsFactory;

        public NotificationsController(IRepository<Notification> notificationRepository, IEmailSender emailSender, 
        INotificationsService notificationsService, ISmsSender smsSender, IExportNotificationsFactory exportNotificationsFactory, IMessageSender messageSender)
        {
            _notificationRepository = notificationRepository;
            _emailSender = emailSender;
            _notificationsService = notificationsService;
            _smsSender = smsSender;
            _exportNotificationsFactory = exportNotificationsFactory;
            _messageSender = messageSender;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string notificationId, string toDestination, string templateName, string platformName, string providerName, string subject, bool? success, bool? hasAttachments, bool? hasParentNotification, int? page, int? pageSize, string sort)
        {
            var filter = PredicateBuilder.New<Notification>(true);

            if (notificationId is not null)
                filter = filter.And(x => x.NotificationId == notificationId);
            
            if (toDestination is not null)
                filter = filter.And(x => x.ToDestination == toDestination);
            
            if (templateName is not null)
                filter = filter.And(x => x.TemplateName == templateName);
            
            if (platformName is not null)
                filter = filter.And(x => x.PlatformName == platformName);
            
            if (providerName is not null)
                filter = filter.And(x => x.ProviderName == providerName);
            
            if (success.HasValue)
                filter = filter.And(x => x.Success == success);
            
            if (hasAttachments.HasValue)
                filter = filter.And(x => x.HasAttachments == hasAttachments);
            
            if (hasParentNotification == true)
                filter = filter.And(x => x.ParentNotificationId != null);
            
            if (hasParentNotification == false)
                filter = filter.And(x => x.ParentNotificationId == null);
            
            if (subject is not null)
                filter = filter.And(x => x.Subject.ToLower().Contains(subject.ToLower()));
            
            var response = await _notificationsService.GetNotifications(filter, owner: CurrentPlatform.Name, page, pageSize, sort);
            return Ok(response);
        }

        [HttpGet("{notificationId}")]
        public async Task<IActionResult> Get([FromRoute] string notificationId)
        {
            var response = await _notificationsService.GetNotificationById(notificationId, owner: CurrentPlatform.Name);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("{notificationId}/resend")]
        public async Task<IActionResult> Resend(string notificationId)
        {
            var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);
            
            Guard.NotificationIsNotNull(notification);
            Guard.NotificationWasCreatedByRequester(notification.CreatedBy, CurrentPlatform.Name);
            Guard.NotificationRequestExists(notification?.Request);
            
            BaseResponse<NotificationSentResponseDto> response = null;
            if (notification.Type == NotificationType.Email)
            {
                var request = notification.Request as SendEmailRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _emailSender.SendEmailAsync(request, CurrentPlatform.Name);
            }
            
            if (notification.Type == NotificationType.SMS)
            {
                var request = notification.Request as SendSmsRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _smsSender.SendSmsAsync(request, CurrentPlatform.Name);
            }
            
            if (notification.Type != NotificationType.Email && 
                notification.Type != NotificationType.SMS)
            {
                var request = notification.Request as SendMessageRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _messageSender.SendMessageAsync(request, CurrentPlatform.Name);

            }
            
            return Ok(response);
        }

        [HttpGet("{notificationId}/content")]
        public async Task<IActionResult> GetNotificationContent(string notificationId)
        {
            var notificationContent = await _notificationsService.GetNotificationById(notificationId, owner: CurrentPlatform.Name);

            var contentResult = new ContentResult { ContentType = "text/html", StatusCode = StatusCodes.Status200OK };

            if (notificationContent == null)
            {
                contentResult.StatusCode = StatusCodes.Status404NotFound;
                contentResult.Content = "Not found";
                return contentResult;
            }

            contentResult.Content = notificationContent.Data.Content;
            return contentResult;
        }

        [HttpGet("{notificationId}/attachments/{fileName}")]
        public async Task<IActionResult> GetFile(string notificationId, string fileName)
        {
            var (file, contentType) = await _notificationsService.GetNotificationAttachment(notificationId, fileName, owner: CurrentPlatform.Name);
            return File(file, contentType);
        }

         [HttpGet("{notificationId}/exports/{format}")]
        public async Task<IActionResult> ExportNotification(string notificationId, [FromRoute] ExportFormat format)
        {

            var exportService = _exportNotificationsFactory.Create(format);
            
            if (exportService == null)
                return NotFound($"The specified format '{format}' doesn't exists");

            var response = await exportService.Export(notificationId, CurrentPlatform.Name);

            if (response == null)
                return NotFound("The specified notificationId doesn't exists");

            var contentResult = new ContentResult
            {
                ContentType = response.ContentType,
                StatusCode = StatusCodes.Status200OK,
                Content = response.Content
            };
            return contentResult;
        }
    }
}