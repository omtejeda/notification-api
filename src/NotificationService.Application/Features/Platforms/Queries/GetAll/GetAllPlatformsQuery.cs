using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Platforms.Queries.GetAll;

public record GetAllPlatformsQuery : IQuery<BaseResponse<IEnumerable<PlatformDto>>>
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string Owner { get; set; } = string.Empty;
}