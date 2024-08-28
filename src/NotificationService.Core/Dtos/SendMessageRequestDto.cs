using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Core.Templates.Attributes;
using NotificationService.Common.Enums;

namespace NotificationService.Core.Dtos
{
    public class SendMessageRequestDto
    {
        [Required]
        public string ToDestination { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType NotificationType { get; set; }

        [Required]
        public string ProviderName { get; set; }

        [Required]
        [ValidateTemplate]
        public TemplateDto Template { get; set; }

        [JsonIgnore]
        public string ParentNotificationId { get; set; }
    }
}