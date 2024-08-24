using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Core.Templates.Attributes;

namespace NotificationService.Contracts.RequestDtos
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