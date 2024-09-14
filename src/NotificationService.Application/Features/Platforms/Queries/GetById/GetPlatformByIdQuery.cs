using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Platforms.Queries.GetById;

public record GetPlatformByIdQuery : IQuery<BaseResponse<PlatformDto>>
{
    public GetPlatformByIdQuery(string? platformId, string? owner)
    {
        PlatformId = platformId;
        Owner = owner;
    }
    
    public string? PlatformId { get; set; }
    public string? Owner { get; set; }
}