using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LinqKit;
using NotificationService.Common.Entities;
using NotificationService.Api.Utils;
using NotificationService.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.Interfaces.Factories;
using NotificationService.Core.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Core.Dtos;

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
            
            var response = await _notificationsService.GetNotifications(filter, owner: Owner, page, pageSize, sort);
            return Ok(response);
        }

        [HttpGet("{notificationId}")]
        public async Task<IActionResult> Get([FromRoute] string notificationId)
        {
            var response = await _notificationsService.GetNotificationById(notificationId, owner: Owner);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("{notificationId}/resend")]
        public async Task<IActionResult> Resend(string notificationId)
        {
            var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == notificationId);

            if (notification == null)
                throw new RuleValidationException("Notification does not exist");
            
            if (notification.CreatedBy != Owner)
                throw new RuleValidationException($"Notification was not created by {Owner}");
            
            if (notification?.Request == null)
                throw new RuleValidationException("Notification request couldn't be found");
            
            FinalResponseDTO<NotificationSentResponseDto> response = null;
            if (notification.Type == NotificationType.Email)
            {
                var request = notification.Request as SendEmailRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _emailSender.SendEmailAsync(request, Owner);
            }
            
            if (notification.Type == NotificationType.SMS)
            {
                var request = notification.Request as SendSmsRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _smsSender.SendSmsAsync(request, Owner);
            }
            
            if (notification.Type != NotificationType.Email && 
                notification.Type != NotificationType.SMS)
            {
                var request = notification.Request as SendMessageRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _messageSender.SendMessageAsync(request, Owner);

            }
            
            return Ok(response);
        }

        [HttpGet("{notificationId}/content")]
        public async Task<IActionResult> GetNotificationContent(string notificationId)
        {
            var notificationContent = await _notificationsService.GetNotificationById(notificationId, owner: Owner);

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
            var (file, contentType) = await _notificationsService.GetNotificationAttachment(notificationId, fileName, owner: Owner);
            return File(file, contentType);
        }

         [HttpGet("{notificationId}/exports/{format}")]
        public async Task<IActionResult> ExportNotification(string notificationId, [FromRoute] ExportFormat format)
        {

            var exportService = _exportNotificationsFactory.Create(format);
            
            if (exportService == null)
                return NotFound($"The specified format '{format}' doesn't exists");

            var response = await exportService.Export(notificationId, Owner);

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