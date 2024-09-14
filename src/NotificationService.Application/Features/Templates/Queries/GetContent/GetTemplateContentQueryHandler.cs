using NotificationService.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Templates.Queries.GetContent;

public class GetTemplateContentQueryHandler(IRepository<Template> templateRepository)
    : IQueryHandler<GetTemplateContentQuery, BaseResponse<TemplateContentDto>>
{
    private readonly IRepository<Template> _templateRepository = templateRepository;

    public async Task<BaseResponse<TemplateContentDto>> Handle(GetTemplateContentQuery request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository
            .FindOneAsync(x => 
                x.TemplateId == request.TemplateId &&
                x.CreatedBy == request.Owner);

        if (template is null) return default!;
        
        var templateContent = new TemplateContentDto(template?.Content);
        return BaseResponse<TemplateContentDto>.Success(templateContent);

    }
}