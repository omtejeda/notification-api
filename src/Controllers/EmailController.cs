using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Services.Interfaces;
using NotificationService.Dtos.Requests;
using NotificationService.Utils;

namespace NotificationService.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class EmailController : ApiController
    {
        private readonly IEmailService _mailService;

        public EmailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendEmailRequestDto request)
        {
            var res = await _mailService.SendEmailAsync(request, Owner);
            return Ok(res);
        }

        [HttpPost("send/attachments")]
        public async Task<IActionResult> SendWithAttachments([FromForm] SendEmailRequestDto request, List<IFormFile> attachments)
        {
            if (!attachments.Any())
            {
                throw new NotificationService.Exceptions.RuleValidationException("Must specify at least one attachment");
            }
            var res = await _mailService.SendEmailAsync(request: request, owner: Owner, attachments: attachments);
            return Ok(res);
        }
    }
}