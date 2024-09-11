using System.Threading.Tasks;
using NotificationService.Core.Dtos;
using NotificationService.Core.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Interfaces
{
    public interface ISmsSender
    {
        Task<BaseResponse<NotificationSentResponseDto>> SendSmsAsync(SendSmsRequestDto request, string owner);
    }
}