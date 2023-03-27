using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Attributes;
using NotificationService.Enums;
using NotificationService.Entities;

namespace NotificationService.Dtos.Requests
{
    public class SendEmailRequestDto
    {
        [Required]
        public string ToEmail { get; set; }

        [Required]
        // [ValidateProvider]
        public string ProviderName { get; set; }

        [Required]
        [ValidateTemplate]
        public TemplateDto Template { get; set; }

        [JsonIgnore]
        public string ParentNotificationId { get; set; }
    }

    public class TemplateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PlatformName { get; set; }

        [Required]
        public ICollection<MetadataDto> Metadata { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Language Language { get; set; } = Language.Default;
    }
}