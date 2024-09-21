using NotificationService.Common.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using LinqKit;
using System.Linq.Expressions;
using NotificationService.Application.Utils;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Platforms.Queries.GetAll;

public class GetAllPlatformsQueryHandler(IPlatformService platformService)
    : IQueryHandler<GetAllPlatformsQuery, BaseResponse<IEnumerable<PlatformDto>>>
{
    private readonly IPlatformService _platformService = platformService;

    public async Task<BaseResponse<IEnumerable<PlatformDto>>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        var predicate = GetPredicateExpression(request);
        return await _platformService.GetPlatforms(predicate, request.Owner, new FilterOptions(request.Page, request.PageSize));
    }

    private Expression<Func<Platform, bool>> GetPredicateExpression(GetAllPlatformsQuery query)
    {
        var predicate = PredicateBuilder.New<Platform>(true);
        
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = predicate.And(x => x.Name == query.Name);
        
        if (query.IsActive is not null)
            predicate = predicate.And(x => x.IsActive == query.IsActive);

        return predicate;
    }
}