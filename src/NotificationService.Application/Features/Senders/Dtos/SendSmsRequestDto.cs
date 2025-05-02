using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Features.Templates.Attributes;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Senders.Dtos;

public class SendSmsRequestDto : BaseNotificationRequest
{
    [Required]
    public string ToPhoneNumber { get; set; } = string.Empty;

    [Required]
    public string ProviderName { get; set; } = string.Empty;

    [Required]
    [ValidateTemplate]
    public TemplateDto Template { get; set; } = new();

    [JsonIgnore]
    public override string? ParentNotificationId { get; set; }
}