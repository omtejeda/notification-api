using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Features.Notifications.Queries.Export;

public record ExportNotificationQuery : IQuery<ExportNotificationResponseDto>
{
    public ExportNotificationQuery(string notificationId, ExportFormat format, string owner)
    {
        NotificationId = notificationId;
        Format = format;
        Owner = owner;
    }
    
    public string NotificationId { get; set; }
    public ExportFormat Format { get; set; }
    public string Owner { get; set; }
}