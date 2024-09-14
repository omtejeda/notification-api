using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using LinqKit;
using System.Linq;
using System.Linq.Expressions;
namespace NotificationService.Application.Features.Templates.Queries.GetAll;

public class GetAllTemplatesQueryHandler(ITemplateService templateService)
    : IQueryHandler<GetAllTemplatesQuery, BaseResponse<IEnumerable<TemplateDto>>>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task<BaseResponse<IEnumerable<TemplateDto>>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
    {
        var predicate = GetPredicateExpression(request);
        return await _templateService.GetTemplates(predicate, request.Owner!, request.Page, request.PageSize);
    }

    private Expression<Func<Template, bool>> GetPredicateExpression(GetAllTemplatesQuery query)
    {
        var predicate = PredicateBuilder.New<Template>(true);
        
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = predicate.And(x => x.Name == query.Name);
        
        if (!string.IsNullOrWhiteSpace(query.Subject))
            predicate = predicate.And(x => x.Subject == query.Subject);
        
        if (!string.IsNullOrWhiteSpace(query.PlatformName))
            predicate = predicate.And(x => x.PlatformName == query.PlatformName);

        return predicate;
    }
}