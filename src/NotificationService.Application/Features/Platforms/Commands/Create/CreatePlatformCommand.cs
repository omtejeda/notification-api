using NotificationService.Common.Dtos;
using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Commands.Create;

public class CreatePlatformCommand : ICommand<BaseResponse<PlatformDto>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
}