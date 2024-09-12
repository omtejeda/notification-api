namespace NotificationService.Application.Contracts.ResponseDtos
{
    public class AttachmentContentDto : AttachmentDto
    {
        public string EncodedContent { get; set; }
    }
}