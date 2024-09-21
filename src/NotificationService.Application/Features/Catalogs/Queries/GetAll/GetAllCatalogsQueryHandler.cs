using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using LinqKit;
using System.Linq.Expressions;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;
namespace NotificationService.Application.Features.Catalogs.Queries.GetAll;

public class GetAllCatalogsQueryHandler(ICatalogService catalogService)
    : IQueryHandler<GetAllCatalogsQuery, BaseResponse<IEnumerable<CatalogDto>>>
{
    private readonly ICatalogService _catalogService = catalogService;
    private const string ElementsDelimiter = ":";

    public async Task<BaseResponse<IEnumerable<CatalogDto>>> Handle(GetAllCatalogsQuery request, CancellationToken cancellationToken)
    {
        var predicate = GetPredicateExpression(request);
        return await _catalogService.GetCatalogs(predicate, owner: request.Owner!, request.Page, request.PageSize);
    }

    private static Expression<Func<Catalog, bool>> GetPredicateExpression(GetAllCatalogsQuery query)
    {
        var predicate = PredicateBuilder.New<Catalog>(true);
        
        if (query.ElementHasKeyValue?.Contains(ElementsDelimiter) ?? false)
        {
            var keyValue = query.ElementHasKeyValue.Split(ElementsDelimiter);
            var key = keyValue.FirstOrDefault();
            var value = keyValue.LastOrDefault();

            predicate.And(x => x.Elements.Any(y => y.Key == key && y.Value == value));
        }

        if (query.ElementHasLabelKeyValue?.Contains(ElementsDelimiter) ?? false)
        {
            var keyValue = query.ElementHasLabelKeyValue.Split(ElementsDelimiter);
            var key = keyValue.FirstOrDefault();
            var value = keyValue.LastOrDefault();

            predicate.And(x => x.Elements.Any(y => y.Labels.Any(z => z.Key == key && z.Value == value)));
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = predicate.And(x => x.Name == query.Name);

        if (query.IsActive.HasValue)
            predicate = predicate.And(x => x.IsActive == query.IsActive);
        
        if (!string.IsNullOrWhiteSpace(query.ElementHasKey))
            predicate = predicate.And(x => x.Elements.Any(y => y.Key == query.ElementHasKey));
        
        if (!string.IsNullOrWhiteSpace(query.ElementHasLabelKey))
            predicate = predicate.And(x => x.Elements.Any(y => y.Labels.Any(z => z.Key == query.ElementHasLabelKey)));

        return predicate;
    }
}