using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.Interfaces.Factories;
using MediatR;
using NotificationService.Application.Features.Notifications.Commands.Resend;
using NotificationService.Application.Features.Notifications.Queries.GetAll;
using NotificationService.Application.Features.Notifications.Queries.GetById;
using NotificationService.Application.Features.Notifications.Queries.GetAttachment;
using NotificationService.Application.Features.Notifications.Queries.Export;

namespace NotificationService.Api.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class NotificationsController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

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
            var query = new GetNotificationByIdQuery(notificationId, CurrentPlatform.Name);
            var response = await _sender.Send(query);
            
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
        public async Task<IActionResult> GetContent(string notificationId)
        {
            var query = new GetNotificationByIdQuery(notificationId, CurrentPlatform.Name);
            var response = await _sender.Send(query);

            var contentResult = new ContentResult
            {
                ContentType = "text/html",
                StatusCode = response is null ? StatusCodes.Status404NotFound : StatusCodes.Status200OK,
                Content = response is null ? "Not found" : response?.Data?.Content
            };

            return contentResult;
        }

        [HttpGet("{notificationId}/attachments/{fileName}")]
        public async Task<IActionResult> GetAttachment(string notificationId, string fileName)
        {
            var query = new GetNotificationAttachmentQuery(notificationId, fileName, CurrentPlatform.Name);
            var (file, contentType) = await _sender.Send(query);
            
            return File(file, contentType);
        }

         [HttpGet("{notificationId}/exports/{format}")]
        public async Task<IActionResult> Export(string notificationId, [FromRoute] ExportFormat format)
        {
            var query = new ExportNotificationQuery(notificationId, format, CurrentPlatform.Name);
            var response = await _sender.Send(query);

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