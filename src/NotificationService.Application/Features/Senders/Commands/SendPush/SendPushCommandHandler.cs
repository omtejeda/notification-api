using MediatR;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Events.NotificationSent;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Domain.Enums;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendPush;

public class SendPushCommandHandler(IPushSender pushSender, IMediator mediator) 
    : ICommandHandler<SendPushCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IPushSender _pushSender = pushSender;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendPushCommand request, CancellationToken cancellationToken)
    {
        var result = await _pushSender.SendPushAsync(request.RequestDto, request.Owner);
        
        await _mediator.Publish(new NotificationSentEvent
        {
            NotificationId = result.Data?.NotificationId, 
            NotificationType = NotificationType.Push,
            ToDestination = request?.RequestDto?.UserId,
            Success = result.Response.Success ?? false
        }, CancellationToken.None);

        return result;
    }
}