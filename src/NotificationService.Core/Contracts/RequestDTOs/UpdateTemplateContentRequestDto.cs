using System.ComponentModel.DataAnnotations;

namespace NotificationService.Core.Contracts.RequestDtos;

public record UpdateTemplateContentRequestDto([Required] string Base64Content);