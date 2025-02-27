using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.Contracts.DTOs.Requests;

public record UpdateTemplateContentRequestDto([Required] string Base64Content);