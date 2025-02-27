using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Contracts.DTOs.Requests;

public class CreateProviderRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public ProviderSettingsDto? Settings { get; set; } = new();

    [Required]
    public bool? IsActive { get; set; } = true;

    [Required]
    public bool? IsPublic { get; set; }

    public ProviderDevSettingsDto? DevSettings { get; set; } = new();
    public bool? SavesAttachments { get; set; } = false;
}