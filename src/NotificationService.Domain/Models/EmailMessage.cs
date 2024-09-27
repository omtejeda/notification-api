using NotificationService.Domain.Dtos;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain.Models
{
    public partial class EmailMessage
    {
        private EmailMessage() {}

        public Email? To { get; private set; }
        public List<Email>? Cc { get; private set; } = [];
        public List<Email>? Bcc { get; private set; } = [];
        public string Subject { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public List<Attachment> Attachments { get; private set; } = [];
        public Dictionary<string,string> Headers { get; private set; } = [];
        public List<MetadataDto> ProvidedMetadata { get; private set; } = [];

    }
}