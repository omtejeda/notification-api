using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Webhooks.Dtos;

namespace NotificationService.Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class WebhooksController : ApiController
    {
        private readonly IWebhooksService _webhookService;

        public WebhooksController(IWebhooksService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost("emails/content")]
        public async Task<IActionResult> SaveEmailContent(SaveEmailContentRequestDto requestDto)
        {
            bool notificationFound = await _webhookService.SaveEmailContent(requestDto.Html,requestDto.Subject, requestDto.Headers);
            
            if(!notificationFound)
                return NoContent();
            return Accepted();
        }
    }
}