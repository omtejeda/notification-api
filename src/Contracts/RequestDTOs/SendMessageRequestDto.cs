using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Core.Templates.Attributes;
using NotificationService.Core.Common.Enums;

namespace NotificationService.Contracts.RequestDtos
{
    public class SendMessageRequestDto : NotificationBaseRequestDto
    {
        [Required]
        public string ToDestination { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType NotificationType { get; set; }
    }
}