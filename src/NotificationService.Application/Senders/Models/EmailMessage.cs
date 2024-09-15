using NotificationService.Domain.Dtos;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Senders.Models
{
    public partial class EmailMessage
    {
        private EmailMessage() {}

        public string To { get; private set; } = string.Empty;
        public List<string> Cc { get; private set; } = [];
        public List<string> Bcc { get; private set; } = [];
        public string Subject { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public List<Attachment> Attachments { get; private set; } = [];
        public Dictionary<string,string> Headers { get; private set; } = [];
        public List<MetadataDto> ProvidedMetadata { get; private set; } = [];

    }
}