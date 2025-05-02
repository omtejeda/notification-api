using MediatR;
using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Features.Senders.Events.NotificationSent;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Domain.Enums;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendMessage;

public class SendMessageCommandHandler(IMessageSender messageSender, IMediator mediator) 
    : ICommandHandler<SendMessageCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IMessageSender _messageSender = messageSender;
    private readonly IMediator _mediator = mediator;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var result = await _messageSender.SendMessageAsync(request.RequestDto, request.Owner);
        
        await _mediator.Publish(new NotificationSentEvent
        {
            NotificationId = result.Data?.NotificationId, 
            NotificationType = request.RequestDto.NotificationType,
            ToDestination = request?.RequestDto?.ToDestination,
            Success = result.Response.Success ?? false
        }, CancellationToken.None);

        return result;
    }
}