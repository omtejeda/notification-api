using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Notifications.Queries.GetAttachment;

public class GetNotificationAttachmentQueryHandler(INotificationsService notificationsService)
    : IQueryHandler<GetNotificationAttachmentQuery, (byte[], string)>
{
    private readonly INotificationsService _notificationsService = notificationsService;

    public async Task<(byte[], string)> Handle(GetNotificationAttachmentQuery request, CancellationToken cancellationToken)
    {
        return await _notificationsService.GetNotificationAttachment(request.NotificationId!, request.FileName!, request.Owner!);
    }
}