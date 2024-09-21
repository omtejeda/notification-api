using NotificationService.Application.Common.Dtos;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Platforms.Commands.Create;

public class CreatePlatformCommand : ICommand<BaseResponse<PlatformDto>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
}