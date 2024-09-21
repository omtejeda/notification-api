using Microsoft.AspNetCore.Http;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Interfaces;

public interface IEmailSender
{
    Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile>? attachments = null);
}