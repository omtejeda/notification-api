using System.ComponentModel.DataAnnotations;

namespace NotificationService.Contracts.RequestDtos;

public record UpdateTemplateContentRequestDto([Required] string Base64Content);