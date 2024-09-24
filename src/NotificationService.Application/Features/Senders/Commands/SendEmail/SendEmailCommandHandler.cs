using NotificationService.Application.Common.Models;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Interfaces;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Senders.Commands.SendEmail;

public class SendEmailCommandHandler(IEmailSender emailSender) 
    : ICommandHandler<SendEmailCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _emailSender.SendEmailAsync(request.RequestDto, request.Owner);
        return result;
    }
}