namespace NotificationService.Application.Contracts.ResponseDtos
{
    public class AttachmentDto
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }
}