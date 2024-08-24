using System.Threading.Tasks;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Senders
{
    public interface ISmsSender
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
    }
}