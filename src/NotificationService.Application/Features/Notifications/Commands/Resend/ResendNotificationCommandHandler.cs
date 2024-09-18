using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Utils;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using System.Reflection.Metadata.Ecma335;

namespace NotificationService.Application.Features.Notifications.Commands.Resend;

public class ResendNotificationCommandHandler
    : ICommandHandler<ResendNotificationCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly IMessageSender _messageSender;

    public ResendNotificationCommandHandler(
        IRepository<Notification> notificationRepository,
        ISmsSender smsSender,
        IEmailSender emailSender,
        IMessageSender messageSender)
    {
        _notificationRepository = notificationRepository;
        _emailSender = emailSender;
        _smsSender = smsSender;
        _messageSender = messageSender;
    }

    public async Task<BaseResponse<NotificationSentResponseDto>> Handle(ResendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.FindOneAsync(x => x.NotificationId == request.NotificationId);
            
        Guard.NotificationIsNotNull(notification);
        Guard.NotificationWasCreatedByRequester(notification.CreatedBy, request.Owner);
        Guard.NotificationRequestExists(notification?.Request);

        SetParentNotificationId(notification.Request, notification.Id);
        
        BaseResponse<NotificationSentResponseDto> response = notification.Type switch
        {
            NotificationType.Email =>
                await _emailSender.SendEmailAsync((SendEmailRequestDto) notification.Request, request.Owner),
            
            NotificationType.SMS =>
                await _smsSender.SendSmsAsync((SendSmsRequestDto) notification.Request, request.Owner),

            _ =>
                await _messageSender.SendMessageAsync((SendMessageRequestDto) notification.Request, request.Owner)
        };
        
        return response;
    }

    private static void SetParentNotificationId(object request, string notificationId)
    {
        if (request is ISendRequest notificationRequest)
        {
            notificationRequest.ParentNotificationId = notificationId;
        }
    }
}