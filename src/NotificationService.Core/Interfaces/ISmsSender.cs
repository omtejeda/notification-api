using System.Threading.Tasks;
using NotificationService.Core.Dtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Interfaces
{
    public interface ISmsSender
    {
        Task<FinalResponseDTO<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
    }
}