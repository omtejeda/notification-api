using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Entities;
using NotificationService.Utils;
using NotificationService.Enums;
using NotificationService.Services.Interfaces;
using NotificationService.Repositories;
using NotificationService.Dtos;
using NotificationService.Exceptions;
using NotificationService.Dtos.Requests;

namespace NotificationService.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class NotificationsController : ApiController
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly INotificationsService _notificationsService;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public NotificationsController(IRepository<Notification> notificationRepository, IEmailService emailService, INotificationsService notificationsService, ISmsService smsService)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _notificationsService = notificationsService;
            _smsService = smsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string notificationId, string toDestination, string templateName, string platformName, string providerName, bool? success, bool? hasAttachments, bool? hasParentNotification, int? page, int? pageSize)
        {
            var response = await _notificationsService.GetNotifications(x => (x.NotificationId == notificationId || notificationId == null) 
                                        && (x.ToDestination == toDestination || toDestination == null) 
                                        && (x.TemplateName == templateName || templateName == null) 
                                        && (x.PlatformName == platformName || platformName == null)
                                        && (x.ProviderName == providerName || providerName == null)
                                        && (x.Success == success || success == null)
                                        && (x.HasAttachments == hasAttachments || hasAttachments == null)
                                        && ( (x.ParentNotificationId  != null && hasParentNotification == true) ||  (x.ParentNotificationId  == null && hasParentNotification == false) ||  hasParentNotification == null )
                                        , owner: Owner, page, pageSize);
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

            if (!(notification.Type == NotificationType.Email || notification.Type == NotificationType.SMS))
                throw new RuleValidationException($"Notification type not suitable {notification.Type.ToString()}");
            
            FinalResponseDTO<Dtos.Responses.NotificationSentResponseDto> response = null;
            if (notification.Type == NotificationType.Email)
            {
                var request = notification.Request as SendEmailRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _emailService.SendEmailAsync(request, Owner);
            }
            
            if (notification.Type == NotificationType.SMS)
            {
                var request = notification.Request as SendSmsRequestDto;
                request.ParentNotificationId = notificationId;
                response = await _smsService.SendSmsAsync(request, Owner);
            }
            
            return Ok(response);
        }
    }
}