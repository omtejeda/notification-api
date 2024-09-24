using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.Contracts.RequestDtos;

public record UpdateTemplateContentRequestDto([Required] string Base64Content);