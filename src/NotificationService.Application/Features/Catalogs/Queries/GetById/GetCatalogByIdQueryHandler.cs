using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.DTOs.Responses;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Catalogs.Queries.GetById;

public class GetCatalogByIdQueryHandler(ICatalogService catalogService)
    : IQueryHandler<GetCatalogByIdQuery, BaseResponse<CatalogDto>>
{
    private readonly ICatalogService _catalogService = catalogService;

    public async Task<BaseResponse<CatalogDto>> Handle(GetCatalogByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _catalogService.GetCatalogById(request.CatalogId, request.Owner);
        if (result?.Data is null) return default!;

        FilterElements(request, result);

        return result;
    }

    private static void FilterElements(GetCatalogByIdQuery request, BaseResponse<CatalogDto> result)
    {
        if (result?.Data is null)
            return;
        
        if (!string.IsNullOrWhiteSpace(request.ElementKey))
            result.Data.Elements = result.Data.Elements.Where(x => x.Key == request.ElementKey).ToList();
        
        if (!string.IsNullOrWhiteSpace(request.ElementKey) && !string.IsNullOrWhiteSpace(request.ElementValue))
            result.Data.Elements = result.Data.Elements.Where(x => x.Key == request.ElementKey && x.Value == request.ElementValue).ToList();

        if (!string.IsNullOrWhiteSpace(request.LabelKey))
            result.Data.Elements = result.Data.Elements.Where(x => x.Labels.Any(z => z.Key == request.LabelKey)).ToList();
        
        if (!string.IsNullOrWhiteSpace(request.LabelKey) && !string.IsNullOrWhiteSpace(request.LabelValue))
            result.Data.Elements = result.Data.Elements.Where(x => x.Labels.Any(z => z.Key == request.LabelKey && z.Value == request.LabelValue)).ToList();
    }
}