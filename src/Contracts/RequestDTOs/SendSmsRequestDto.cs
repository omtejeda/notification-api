using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Core.Templates.Attributes;

namespace NotificationService.Contracts.RequestDtos
{
    public class SendSmsRequestDto : NotificationBaseRequestDto
    {
        [Required]
        public string ToPhoneNumber { get; set; }
    }
}