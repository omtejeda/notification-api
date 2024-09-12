using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
namespace NotificationService.Application.Features.Platforms.Commands.CreatePlatform;

public class CreatePlatformCommand : ICommand<BaseResponse<PlatformDto>>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Owner { get; set; }
}