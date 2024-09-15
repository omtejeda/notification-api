using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.ResponseDtos;

namespace NotificationService.Application.Features.Catalogs.Queries.GetById;

public record GetCatalogByIdQuery :IQuery<BaseResponse<CatalogDto>>
{    
    public string? CatalogId { get; init; }
    public string? ElementKey { get; init; }
    public string? ElementValue { get; init; }
    public string? LabelKey { get; init; }
    public string? LabelValue { get; init; }
    public string? Owner { get; init; }
}