using NotificationService.Common.Dtos;
using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Application.Features.Platforms.Commands.Create;

public class CreatePlatformCommand : ICommand<BaseResponse<PlatformDto>>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Owner { get; set; }
}