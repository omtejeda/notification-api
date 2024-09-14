using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.Interfaces.Factories;
using NotificationService.Application.Features.Notifications.Commands.Resend;
using MediatR;
using NotificationService.Application.Features.Notifications.Queries.GetAll;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class NotificationsController : ApiController
    {
        private readonly INotificationsService _notificationsService;
        private readonly IExportNotificationsFactory _exportNotificationsFactory;

        private readonly ISender _sender;

        public NotificationsController(
            INotificationsService notificationsService,
            IExportNotificationsFactory exportNotificationsFactory,
            ISender sender)
        {
            _notificationsService = notificationsService;
            _exportNotificationsFactory = exportNotificationsFactory;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsQuery query)
        {
            query.SetOwner(CurrentPlatform.Name);
            var response = await _sender.Send(query);
            return Ok(response);
        }

        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetById([FromRoute] string notificationId)
        {
            var response = await _notificationsService.GetNotificationById(notificationId, owner: CurrentPlatform.Name);
            return GetActionResult(response);
        }

        [HttpPost("{notificationId}/resend")]
        public async Task<IActionResult> Resend(string notificationId)
        {
            var command = new ResendNotificationCommand(notificationId, CurrentPlatform.Name);
            var response = await _sender.Send(command);

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