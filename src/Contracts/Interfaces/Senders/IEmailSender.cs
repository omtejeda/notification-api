using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Senders
{
    public interface IEmailSender
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null);
    }
}