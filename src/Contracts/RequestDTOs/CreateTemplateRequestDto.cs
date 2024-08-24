using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using NotificationService.Core.Common.Enums;
using System.Text.Json.Serialization;

namespace NotificationService.Contracts.RequestDtos
{
    public class CreateTemplateRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Language Language { get; set; }

        [Required]
        public string NotificationType { get; set; }
        
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ICollection<MetadataRequired> Metadata { get; set; }
        public ICollection<TemplateLabelDTO> Labels { get; set; }
    }

    public class MetadataRequired
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsRequired { get; set; }
    }

    public class TemplateLabelDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string CatalogNameToCheckAgainst { get; set; }
    }
}