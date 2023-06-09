using System;
using System.Collections.Generic;
namespace NotificationService.Dtos
{
    public class NotificationDTO
    {
        public string NotificationId { get; set; }
        public string Type { get; set; }
        public string ToDestination { get; set; }
        public string TemplateName { get; set; }
        public string PlatformName { get; set; }
        public string ProviderName { get; set; }
        public DateTime? Date { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public bool? HasAttachments { get; set; }
        public ICollection<AttachmentDTO> Attachments { get; set; }
        public object Request { get; set; }
        public string ParentNotificationId { get; set; }
    }
}