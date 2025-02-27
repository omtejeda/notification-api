using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.Contracts.DTOs.Requests;

public class CreatePlatformRequestDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }
}