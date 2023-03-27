using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NotificationService.Dtos;
using NotificationService.Interfaces;
using NotificationService.Dtos.Requests;
using NotificationService.Dtos.Responses;
using MimeKit;

namespace NotificationService.Services.Interfaces
{
    public interface IEmailService
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendEmailAsync(SendEmailRequestDto request, string owner, List<IFormFile> attachments = null);
    }
}