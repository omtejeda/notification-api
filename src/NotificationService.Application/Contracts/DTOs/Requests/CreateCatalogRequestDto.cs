using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Contracts.DTOs.Responses;

namespace NotificationService.Application.Contracts.DTOs.Requests;

public class CreateCatalogRequestDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public ICollection<ElementDto> Elements { get; set; } = [];
}