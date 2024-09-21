using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Interfaces;

public interface ISmsSender
{
    Task<BaseResponse<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
}