using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Notifications.Queries.GetById;

public record GetNotificationByIdQuery : IQuery<BaseResponse<NotificationDetailDto>>
{
    public GetNotificationByIdQuery(string notificationId, string owner)
    {
        NotificationId = notificationId;
        Owner = owner;
    }
    
    public string NotificationId { get; set; }
    public string Owner { get; set; }
}