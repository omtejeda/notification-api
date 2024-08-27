using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.RequestDtos
{
    public class CreateCatalogRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public ICollection<ElementDTO> Elements { get; set; }
    }
}