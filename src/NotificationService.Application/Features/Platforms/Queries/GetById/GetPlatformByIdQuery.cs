using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Platforms.Queries.GetById;

public record GetPlatformByIdQuery : IQuery<BaseResponse<PlatformDto>>
{
    public GetPlatformByIdQuery(string platformId, string owner)
    {
        PlatformId = platformId;
        Owner = owner;
    }
    
    public string PlatformId { get; set; }
    public string Owner { get; set; }
}