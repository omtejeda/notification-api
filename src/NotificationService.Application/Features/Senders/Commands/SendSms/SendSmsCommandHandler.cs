using MediatR;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Events.NotificationSent;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Domain.Enums;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendSms;

public class SendSmsCommandHandler(ISmsSender smsSender, IMediator mediator) 
    : ICommandHandler<SendSmsCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly ISmsSender _smsSender = smsSender;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendSmsCommand request, CancellationToken cancellationToken)
    {
        var result = await _smsSender.SendSmsAsync(request.RequestDto, request.Owner);

        await _mediator.Publish(new NotificationSentEvent
        {
            NotificationId = result.Data?.NotificationId, 
            NotificationType = NotificationType.Email,
            ToDestination = request?.RequestDto?.ToPhoneNumber,
            Success = result.Response.Success ?? false
        }, CancellationToken.None);

        return result;
    }
}