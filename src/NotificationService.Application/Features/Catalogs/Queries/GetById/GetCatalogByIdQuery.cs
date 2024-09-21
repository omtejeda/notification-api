using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Catalogs.Queries.GetById;

public record GetCatalogByIdQuery :IQuery<BaseResponse<CatalogDto>>
{    
    public string CatalogId { get; init; } = string.Empty;
    public string? ElementKey { get; init; }
    public string? ElementValue { get; init; }
    public string? LabelKey { get; init; }
    public string? LabelValue { get; init; }
    public string Owner { get; init; } = string.Empty;
}