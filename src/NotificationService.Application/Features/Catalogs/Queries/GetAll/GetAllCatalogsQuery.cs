using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Catalogs.Queries.GetAll;

public record GetAllCatalogsQuery : IQuery<BaseResponse<IEnumerable<CatalogDto>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public string? ElementHasKey { get; init; }
    public string? ElementHasKeyValue { get; init; }
    public string? ElementHasLabelKey { get; init; }
    public string? ElementHasLabelKeyValue { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
    public string? Owner { get; init; }
}