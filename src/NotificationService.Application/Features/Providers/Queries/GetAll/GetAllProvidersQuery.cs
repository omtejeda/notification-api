using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Providers.Queries.GetAll;

public record GetAllProvidersQuery : IQuery<BaseResponse<IEnumerable<ProviderDto>>>
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string Owner { get; set; } = string.Empty;
}