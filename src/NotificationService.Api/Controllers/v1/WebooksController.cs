using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Features.Webhooks.Commands.SaveEmailContent;

namespace NotificationService.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class WebhooksController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpPost("emails/content")]
        public async Task<IActionResult> SaveEmailContent([FromBody] SaveEmailContentCommand command)
        {
            bool notificationFound = await _sender.Send(command);
            return notificationFound ? Accepted() : NoContent();
        }
    }
}