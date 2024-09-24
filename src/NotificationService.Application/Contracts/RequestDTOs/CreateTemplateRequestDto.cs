using System.ComponentModel.DataAnnotations;
using NotificationService.Domain.Enums;
using System.Text.Json.Serialization;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Contracts.RequestDtos;

public class CreateTemplateRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Language Language { get; set; }

    [Required]
    public string? NotificationType { get; set; }
    
    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public ICollection<MetadataRequired> Metadata { get; set; } = Array.Empty<MetadataRequired>();
    public ICollection<TemplateLabelDto> Labels { get; set; } = Array.Empty<TemplateLabelDto>();
}

public class MetadataRequired
{
    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool IsRequired { get; set; }
}