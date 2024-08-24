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
    public class SmsController : ApiController
    {
        private readonly ISmsSender _smsSender;
        public SmsController(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendSmsRequestDto request)
        {
            var res = await _smsSender.SendSmsAsync(request, Owner);
            return Ok(res);
        }
    }
}