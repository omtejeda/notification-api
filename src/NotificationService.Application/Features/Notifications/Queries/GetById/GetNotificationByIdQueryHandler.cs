using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Notifications.Queries.GetById;

public class GetNotificationByIdQueryHandler(INotificationsService notificationsService)
    : IQueryHandler<GetNotificationByIdQuery, BaseResponse<NotificationDetailDto>>
{
    private readonly INotificationsService _notificationsService = notificationsService;

    public async Task<BaseResponse<NotificationDetailDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _notificationsService.GetNotificationById(request.NotificationId, request.Owner);
    }
}