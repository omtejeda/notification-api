using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using LinqKit;
using System.Linq;
using System.Linq.Expressions;
namespace NotificationService.Application.Features.Providers.Queries.GetAll;

public class GetAllProvidersQueryHandler(IProviderService providerService)
    : IQueryHandler<GetAllProvidersQuery, BaseResponse<IEnumerable<ProviderDto>>>
{
    private readonly IProviderService _providerService = providerService;

    public async Task<BaseResponse<IEnumerable<ProviderDto>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
    {
        var predicate = GetPredicateExpression(request);
        return await _providerService.GetProviders(predicate, request.Owner, request.Page, request.PageSize);
    }

    private Expression<Func<Provider, bool>> GetPredicateExpression(GetAllProvidersQuery query)
    {
        var predicate = PredicateBuilder.New<Provider>(true);
        Enum.TryParse(query.Type, out ProviderType providerType);
        
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = predicate.And(x => x.Name == query.Name);
        
        if (!string.IsNullOrWhiteSpace(query.Type))
            predicate = predicate.And(x => x.Type == providerType);

        return predicate;
    }
}