using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null);
    }
}