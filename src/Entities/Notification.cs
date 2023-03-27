using System;
using System.Collections.Generic;
using NotificationService.Enums;

namespace NotificationService.Entities
{
    public class Notification : BaseEntity
    {
        public string NotificationId { get; set; }
        public NotificationType Type { get; set; }
        public string ToDestination { get; set; }
        public string TemplateName { get; set; }
        public string PlatformName { get; set; }
        public string ProviderName { get; set; }
        public DateTime? Date { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public bool? HasAttachments { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public object Request { get; set; }
        public string ParentNotificationId { get; set; }
    }
}