using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Dtos;
using NotificationService.Application.Features.Templates.Attributes;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Senders.Dtos;

public class SendEmailRequestDto : BaseNotificationRequest
{
    [Required]
    public string ToEmail { get; set; } = string.Empty;

    public ICollection<string> CcEmails { get; set; } = [];
    public ICollection<string> BccEmails { get; set; } = [];

    [Required]
    public string ProviderName { get; set; } = string.Empty;

    [Required]
    [ValidateTemplate]
    public TemplateDto Template { get; set; } = new();

    [JsonIgnore]
    public override string? ParentNotificationId { get; set; }
}

public class TemplateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PlatformName { get; set; } = string.Empty;

    [Required]
    public ICollection<MetadataDto> Metadata { get; set; } = [];

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Language Language { get; set; } = Language.Default;
}