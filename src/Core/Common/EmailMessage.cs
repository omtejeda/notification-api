using NotificationService.Contracts.RequestDtos;
using NotificationService.Core.Common.Entities;
using System.Collections.Generic;

namespace NotificationService.Core.Common
{
    public partial class EmailMessage
    {
        private EmailMessage() {}

        public string To { get; private set; }
        public List<string> Cc { get; private set; } = new List<string>();
        public List<string> Bcc { get; private set; } = new List<string>();
        public string Subject { get; private set; }
        public string Content { get; private set; }
        public List<Attachment> Attachments { get; private set; } = new List<Attachment>();
        public Dictionary<string,string> Headers { get; private set; } = new Dictionary<string, string>();
        public List<MetadataDto> ProvidedMetadata { get; private set; } = new List<MetadataDto>();

    }
}