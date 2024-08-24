using System.Collections.Generic;
namespace NotificationService.Contracts.ResponseDtos
{
    public class NotificationDetailDto : NotificationDTO
    {
        public object Request { get; set; }
        public string Content {get; set;}
        public new ICollection<AttachmentDTO> Attachments { get; set; }

    }
}