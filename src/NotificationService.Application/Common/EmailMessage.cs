﻿using NotificationService.Domain.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using System.Collections.Generic;

namespace NotificationService.Application.Common
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