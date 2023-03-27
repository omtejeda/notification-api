using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;
using NotificationService.Dtos.Requests;
using NotificationService.Utils;

namespace NotificationService.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class SmsController : ApiController
    {
        private readonly ISmsService _smsService;
        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendSmsRequestDto request)
        {
            var res = await _smsService.SendSmsAsync(request, Owner);
            return Ok(res);
        }
    }
}