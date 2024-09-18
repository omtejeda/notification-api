using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Domain.Enums;
using MediatR;
using NotificationService.Application.Features.Notifications.Commands.Resend;
using NotificationService.Application.Features.Notifications.Queries.GetAll;
using NotificationService.Application.Features.Notifications.Queries.GetById;
using NotificationService.Application.Features.Notifications.Queries.GetAttachment;
using NotificationService.Application.Features.Notifications.Queries.Export;
using Swashbuckle.AspNetCore.Annotations;

namespace NotificationService.Api.Controllers.v1
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class NotificationsController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [SwaggerOperation("Retrieves a list of all notifications")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsQuery query)
        {
            query.SetOwner(CurrentPlatform.Name);
            var response = await _sender.Send(query);
            
            return Ok(response);
        }
        [SwaggerOperation("Fetches details of a specific notification by its ID")]
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetById([FromRoute] string notificationId)
        {
            var query = new GetNotificationByIdQuery(notificationId, CurrentPlatform.Name);
            var response = await _sender.Send(query);
            
            return GetActionResult(response);
        }

        [SwaggerOperation("Resends a notification based on the original notification with the specified ID")]
        [HttpPost("{notificationId}/resend")]
        public async Task<IActionResult> Resend(string notificationId)
        {
            var command = new ResendNotificationCommand(notificationId, CurrentPlatform.Name);
            var response = await _sender.Send(command);

            return Ok(response);
        }

        [SwaggerOperation("Retrieves the content of a specific notification")]
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

        [SwaggerOperation("Downloads an attachment from a notification by its file name")]
        [HttpGet("{notificationId}/attachments/{fileName}")]
        public async Task<IActionResult> GetAttachment(string notificationId, string fileName)
        {
            var query = new GetNotificationAttachmentQuery(notificationId, fileName, CurrentPlatform.Name);
            var (file, contentType) = await _sender.Send(query);
            
            return File(file, contentType);
        }

        [SwaggerOperation("Exports a notification's data in the specified format (e.g., Eml)")]
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