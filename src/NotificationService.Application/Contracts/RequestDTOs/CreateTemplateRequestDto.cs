using System.ComponentModel.DataAnnotations;
using NotificationService.Domain.Enums;
using NotificationService.Common.Dtos;
using System.Text.Json.Serialization;

namespace NotificationService.Application.Contracts.RequestDtos
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
        public ICollection<MetadataRequired> Metadata { get; set; } = Array.Empty<MetadataRequired>();
        public ICollection<TemplateLabelDto> Labels { get; set; } = Array.Empty<TemplateLabelDto>();
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
}