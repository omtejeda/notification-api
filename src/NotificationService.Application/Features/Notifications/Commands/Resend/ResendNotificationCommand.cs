using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Notifications.Commands.Resend;

public record ResendNotificationCommand(string? NotificationId, string? Owner) : ICommand<BaseResponse<NotificationSentResponseDto>>;