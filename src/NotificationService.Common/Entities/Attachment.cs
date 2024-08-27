using Microsoft.AspNetCore.Http;

namespace NotificationService.Common.Entities
{
    public class Attachment
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public IFormFile FormFile { get; set; }
    }
}