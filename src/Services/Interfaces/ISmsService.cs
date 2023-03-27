using System.Threading.Tasks;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Dtos.Responses;

namespace NotificationService.Services.Interfaces
{
    public interface ISmsService
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
    }
}