using System.Threading.Tasks;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Interfaces
{
    public interface ISmsSender
    {
        Task<BaseResponse<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
    }
}