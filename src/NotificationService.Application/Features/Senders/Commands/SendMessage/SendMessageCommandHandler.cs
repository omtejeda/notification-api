using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Interfaces;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendMessage;

public class SendMessageCommandHandler(IMessageSender messageSender) 
    : ICommandHandler<SendMessageCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IMessageSender _messageSender = messageSender;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var result = await _messageSender.SendMessageAsync(request.RequestDto, request.Owner);
        return result;
    }
}