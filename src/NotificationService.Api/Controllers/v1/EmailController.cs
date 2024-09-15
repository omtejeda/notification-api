using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Api.Utils;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Utils;

namespace NotificationService.Api.Controllers.v1
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class EmailController : ApiController
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendEmailRequestDto request)
        {
            var res = await _emailSender.SendEmailAsync(request, CurrentPlatform.Name);
            return Ok(res);
        }

        [HttpPost("send/attachments")]
        public async Task<IActionResult> SendWithAttachments([FromForm] SendEmailRequestDto request, List<IFormFile> attachments)
        {
            Guard.HasAttachments(attachments);
            var res = await _emailSender.SendEmailAsync(request: request, owner: CurrentPlatform.Name, attachments: attachments);
            return Ok(res);
        }
    }
}