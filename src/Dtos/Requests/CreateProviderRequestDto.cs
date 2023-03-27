using System.ComponentModel.DataAnnotations;

namespace NotificationService.Dtos.Requests
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
    }
}