using System.Threading.Tasks;
using NotificationService.Core.Dtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Interfaces
{
    public interface IMessageSender
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendMessageAsync(SendMessageRequestDto request, string owner);
    }
}