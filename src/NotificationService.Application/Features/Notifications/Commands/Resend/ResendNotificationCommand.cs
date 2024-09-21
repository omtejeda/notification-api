using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Notifications.Commands.Resend;

public record ResendNotificationCommand(string NotificationId, string Owner) : ICommand<BaseResponse<NotificationSentResponseDto>>;