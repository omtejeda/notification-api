using System.ComponentModel.DataAnnotations;
using NotificationService.Core.Providers.Dtos;

namespace NotificationService.Contracts.RequestDtos
{
    public class CreateProviderRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public ProviderSettingsDTO Settings { get; set; }

        [Required]
        public bool? IsActive { get; set; } = true;

        [Required]
        public bool? IsPublic { get; set; }

        public ProviderDevSettingsDTO DevSettings { get; set; }
        public bool? SavesAttachments { get; set; } = false;
    }
}