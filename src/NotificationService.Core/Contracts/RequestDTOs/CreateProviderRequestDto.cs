using System.ComponentModel.DataAnnotations;
using NotificationService.Common.Dtos;

namespace NotificationService.Core.Contracts.RequestDtos
{
    public class CreateProviderRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public ProviderSettingsDto Settings { get; set; }

        [Required]
        public bool? IsActive { get; set; } = true;

        [Required]
        public bool? IsPublic { get; set; }

        public ProviderDevSettingsDto DevSettings { get; set; }
        public bool? SavesAttachments { get; set; } = false;
    }
}