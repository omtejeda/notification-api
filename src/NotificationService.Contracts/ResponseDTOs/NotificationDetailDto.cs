using System.Collections.Generic;
namespace NotificationService.Contracts.ResponseDtos
{
    public class NotificationDetailDto : NotificationDto
    {
        public object Request { get; set; }
        public string Content {get; set;}
        public new ICollection<AttachmentDto> Attachments { get; set; }

    }
}