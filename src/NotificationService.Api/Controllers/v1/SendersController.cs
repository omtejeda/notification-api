using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Application.Features.Senders.Commands.SendEmail;
using NotificationService.Application.Features.Senders.Commands.SendMessage;
using NotificationService.Application.Features.Senders.Commands.SendSms;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Common.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using NotificationService.Application.Features.Senders.Commands.SendPush;
using NotificationService.Api.Extensions;

namespace NotificationService.Api.Controllers.v1;

[ApiController]
[ApiVersion(ApiVersions.v1)]
[Route("")]
public class SendersController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;
    
    [SwaggerOperation("Send an email notification")]
    [HttpPost("email/send")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto request)
    {
        var command = new SendEmailCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return response.ToActionResult();
    }

    [SwaggerOperation("Send an email notification with attachments")]
    [HttpPost("email/send/attachments")]
    public async Task<IActionResult> SendEmailWithAttachments([FromForm] SendEmailRequestDto request, List<IFormFile> attachments)
    {
        Guard.HasAttachments(attachments);
        var command = new SendEmailCommand(request, CurrentPlatform.Name, attachments);
        var response = await _sender.Send(command);
        
        return response.ToActionResult();
    }

    [SwaggerOperation("Send a general message notification (e.g., WhatsApp, Push) using a though HTTP / API calls")]
    [HttpPost("message/send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDto request)
    {
        var command = new SendMessageCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return response.ToActionResult();
    }

    [SwaggerOperation("Send an SMS notification to the specified phone number")]
    [HttpPost("sms/send")]
    public async Task<IActionResult> SendSms([FromBody] SendSmsRequestDto request)
    {
        var command = new SendSmsCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return response.ToActionResult();
    }

    [SwaggerOperation("Sends a push notification")]
    [HttpPost("push/send")]
    public async Task<IActionResult> SendPush([FromBody] SendPushRequestDto request)
    {
        var command = new SendPushCommand(request, CurrentPlatform.Name);
        var response = await _sender.Send(command);
        
        return response.ToActionResult();
    }
}