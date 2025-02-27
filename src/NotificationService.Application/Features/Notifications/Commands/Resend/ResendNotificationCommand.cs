using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Notifications.Commands.Resend;

public record ResendNotificationCommand(string NotificationId, string Owner) : ICommand<BaseResponse<NotificationSentResponseDto>>;