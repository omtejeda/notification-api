using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route("")]
public class SendersController(
    IEmailSender emailSender,
    IMessageSender messageSender,
    ISmsSender smsSender) 
    : ApiController
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IMessageSender _messageSender = messageSender;
    private readonly ISmsSender _smsSender = smsSender;
    
    [SwaggerOperation("Sends an email notification")]
    [HttpPost("email/send")]
    public async Task<IActionResult> Send([FromBody] SendEmailRequestDto request)
    {
        var res = await _emailSender.SendEmailAsync(request, CurrentPlatform.Name);
        return Ok(res);
    }

    [SwaggerOperation("Sends an email notification with attachments")]
    [HttpPost("email/send/attachments")]
    public async Task<IActionResult> SendWithAttachments([FromForm] SendEmailRequestDto request, List<IFormFile> attachments)
    {
        Guard.HasAttachments(attachments);
        var res = await _emailSender.SendEmailAsync(request: request, owner: CurrentPlatform.Name, attachments: attachments);
        return Ok(res);
    }

    [SwaggerOperation("Sends a general message notification (e.g., WhatsApp, Push) using a HTTP Client Provider")]
    [HttpPost("message/send")]
    public async Task<IActionResult> Send([FromBody] SendMessageRequestDto request)
    {
        var res = await _messageSender.SendMessageAsync(request, CurrentPlatform.Name);
        return Ok(res);
    }

    [SwaggerOperation("Sends an SMS notification to the specified phone number")]
    [HttpPost("sms/send")]
    public async Task<IActionResult> Send([FromBody] SendSmsRequestDto request)
    {
        var res = await _smsSender.SendSmsAsync(request, CurrentPlatform.Name);
        return Ok(res);
    }
}