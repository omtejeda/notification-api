using System.Threading.Tasks;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Interfaces
{
    public interface IMessageSender
    {
        Task<BaseResponse<NotificationSentResponseDto>> SendMessageAsync(SendMessageRequestDto request, string owner);
    }
}