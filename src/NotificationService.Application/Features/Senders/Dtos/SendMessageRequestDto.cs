using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using NotificationService.Domain.Enums;
using NotificationService.Application.Features.Templates.Attributes;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Senders.Dtos;

public class SendMessageRequestDto : BaseNotificationRequest
{
    [Required]
    public string ToDestination { get; set; } = string.Empty;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationType NotificationType { get; set; }

    [Required]
    public string ProviderName { get; set; } = string.Empty;

    [Required]
    [ValidateTemplate]
    public TemplateDto Template { get; set; } = new();

    [JsonIgnore]
    public override string? ParentNotificationId { get; set; }
}