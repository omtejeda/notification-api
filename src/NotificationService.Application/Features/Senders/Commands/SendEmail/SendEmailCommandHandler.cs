using MediatR;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Senders.Events.NotificationSent;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendEmail;

public class SendEmailCommandHandler(IEmailSender emailSender, IMediator mediator) 
    : ICommandHandler<SendEmailCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _emailSender.SendEmailAsync(request.RequestDto, request.Owner);

        await _mediator.Publish(new NotificationSentEvent
        {
            NotificationId = result.Data?.NotificationId, 
            NotificationType = NotificationType.Email,
            ToDestination = request?.RequestDto?.ToEmail,
            Success = result.Response.Success ?? false
        });
        
        return result;
    }
}