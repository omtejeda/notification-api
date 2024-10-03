using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Utils;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Application.Common.Models;
using MediatR;
using NotificationService.Application.Features.Notifications.Events.Resent;

namespace NotificationService.Application.Features.Notifications.Commands.Resend;

public class ResendNotificationCommandHandler(
    IRepository<Notification> notificationRepository,
    ISmsSender smsSender,
    IEmailSender emailSender,
    IMessageSender messageSender,
    IMediator mediator)
        : ICommandHandler<ResendNotificationCommand, BaseResponse<NotificationSentResponseDto>>
{
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ISmsSender _smsSender = smsSender;
    private readonly IMessageSender _messageSender = messageSender;
    private readonly IMediator _mediator = mediator;

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
        
        await _mediator.Publish(new NotificationResentEvent(request.NotificationId, response.Response.Success), CancellationToken.None);
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