using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.Contracts.RequestDtos
{
    public class CreatePlatformRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}