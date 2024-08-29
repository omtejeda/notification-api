using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotificationService.Core.Dtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<BaseResponse<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null);
    }
}