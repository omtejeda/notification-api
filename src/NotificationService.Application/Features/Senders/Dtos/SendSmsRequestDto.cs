using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Application.Features.Templates.Attributes;

namespace NotificationService.Application.Features.Senders.Dtos;

public class SendSmsRequestDto : ISendRequest
{
    [Required]
    public string ToPhoneNumber { get; set; } = string.Empty;

    [Required]
    public string ProviderName { get; set; } = string.Empty;

    [Required]
    [ValidateTemplate]
    public TemplateDto Template { get; set; } = new();

    [JsonIgnore]
    public string? ParentNotificationId { get; set; }
}