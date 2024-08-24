using System.Collections.Generic;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Core.Common.Enums;

namespace NotificationService.Core.Templates.Models
{
    public class RuntimeTemplate
    {
        public string Name { get; set; }
        public string PlatformName { get; set; }
        public Language Language { get; set; }
        public List<MetadataDto> ProvidedMetadata { get; set; } = new List<MetadataDto>();
        public string Content { get; set; }
        public string Subject { get; set; }
    }
}