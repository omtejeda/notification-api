using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Templates.Attributes;

namespace NotificationService.Application.Dtos
{
    public class SendSmsRequestDto
    {
        [Required]
        public string ToPhoneNumber { get; set; }

        [Required]
        public string ProviderName { get; set; }

        [Required]
        [ValidateTemplate]
        public TemplateDto Template { get; set; }

        [JsonIgnore]
        public string ParentNotificationId { get; set; }
    }
}