using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Notifications.Queries.GetById;

public record GetNotificationByIdQuery : IQuery<BaseResponse<NotificationDetailDto>>
{
    public GetNotificationByIdQuery(string? notificationId, string? owner)
    {
        NotificationId = notificationId;
        Owner = owner;
    }
    
    public string? NotificationId { get; set; }
    public string? Owner { get; set; }
}