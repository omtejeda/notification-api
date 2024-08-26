using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NotificationService.Core.Templates.Attributes;

namespace NotificationService.Contracts.RequestDtos
{
    public abstract class NotificationBaseRequestDto
    {
        [Required]
        public string ProviderName { get; set; }

        [Required]
        [ValidateTemplate]
        public TemplateDto Template { get; set; }

        [JsonIgnore]
        public string ParentNotificationId { get; set; }
    }
}