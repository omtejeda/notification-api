using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Templates.Attributes;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Dtos
{
    public class SendMessageRequestDto : ISendRequest
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
        public string? ParentNotificationId { get; set; }
    }
}