using System.Threading.Tasks;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Senders
{
    public interface IMessageSender
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendMessageAsync(SendMessageRequestDto request, string owner);
    }
}