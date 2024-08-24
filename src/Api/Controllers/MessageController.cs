using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Contracts.Interfaces.Senders;
using NotificationService.Contracts.RequestDtos;

namespace NotificationService.Api.Controllers
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
            var res = await _messageSender.SendMessageAsync(request, Owner);
            return Ok(res);
        }
    }
}