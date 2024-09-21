using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Notifications.Queries.GetAttachment;

public record GetNotificationAttachmentQuery : IQuery<(byte[], string)>
{
    public GetNotificationAttachmentQuery(string notificationId, string fileName, string owner)
    {
        NotificationId = notificationId;
        FileName = fileName;
        Owner = owner;
    }
    
    public string NotificationId { get; set; }
    public string FileName { get; set; }
    public string Owner { get; set; }
}