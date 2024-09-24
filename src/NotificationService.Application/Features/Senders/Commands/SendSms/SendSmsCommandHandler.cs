using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Interfaces;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendSms;

public class SendSmsCommandHandler(ISmsSender smsSender) 
    : ICommandHandler<SendSmsCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly ISmsSender _smsSender = smsSender;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendSmsCommand request, CancellationToken cancellationToken)
    {
        var result = await _smsSender.SendSmsAsync(request.RequestDto, request.Owner);
        return result;
    }
}