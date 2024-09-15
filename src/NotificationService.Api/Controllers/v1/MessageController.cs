using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Interfaces;

namespace NotificationService.Api.Controllers.v1
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class MessageController : ApiController
    {
        private IMessageSender _messageSender;
        public MessageController(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendMessageRequestDto request)
        {
            var res = await _messageSender.SendMessageAsync(request, CurrentPlatform.Name);
            return Ok(res);
        }
    }
}