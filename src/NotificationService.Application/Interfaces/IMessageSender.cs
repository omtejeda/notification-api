using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Interfaces;

public interface IMessageSender
{
    Task<BaseResponse<NotificationSentResponseDto>> SendMessageAsync(SendMessageRequestDto request, string owner);
}